using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Services.Stats.Model;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetPlayersStatsQuery
    {
        public record Query(IEnumerable<Player> Players, IEnumerable<PlayerMatchRecord> MatchRecords, IDictionary<string, decimal> RankTable) : IRequest<IEnumerable<PlayerStatItemResponse>>;

        public class GetPlayersStatsHandler : IRequestHandler<Query, IEnumerable<PlayerStatItemResponse>>
        {
            private readonly IRepository<Match> _matches;
            private readonly IRepository<Player> _players;
            private readonly IMediator _mediator;

            public GetPlayersStatsHandler(IRepository<Match> matches,
                IRepository<Player> players,
                IMediator mediator)
            {
                _matches = matches;
                _players = players;
                _mediator = mediator;
            }

            public async Task<IEnumerable<PlayerStatItemResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var players = request.Players ?? await _players.GetAllAsync();
                var matchRecords = request.MatchRecords ?? (await _matches.GetAllAsync()).Select(x => x.PlayerRecords).SelectMany(x => x);
                var rankTable = request.RankTable ?? await _mediator.Send(new GetRanksQuery.Query(players));

                var groupedMatchRecords = from record in matchRecords
                                          where record.PlayerId.HasValue
                                          group record by record.PlayerId.Value into g
                                          select g;

                var stats = from p in players
                            join gmr in groupedMatchRecords on p.Id equals gmr.Key into jgmr
                            from gmr in jgmr.DefaultIfEmpty()
                            select PlayerStatItemResponse.FromMatchRecordGrouping(p, gmr, rankTable);

                var response = stats
                    .OrderByDescending(s => s.MMR)
                    .ToList();

                int i = 1;
                foreach (var r in response)
                {
                    r.Position = i++;
                }

                return response;
            }
        }
    }
}
