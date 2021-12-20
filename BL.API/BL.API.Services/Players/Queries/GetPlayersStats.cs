using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetPlayersStats
    {
        public record Query() : IRequest<IEnumerable<PlayerStatItemResponse>>;

        public class GetPlayersStatsHandler : IRequestHandler<Query, IEnumerable<PlayerStatItemResponse>>
        {
            private readonly IRepository<PlayerMatchRecord> _matchRecords;
            private readonly IRepository<Player> _players;

            public GetPlayersStatsHandler(IRepository<PlayerMatchRecord> matchRecords, IRepository<Player> players)
            {
                _matchRecords = matchRecords;
                _players = players;
            }

            public async Task<IEnumerable<PlayerStatItemResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var players = await _players.GetAllAsync();
                var matchRecords = await _matchRecords.GetAllAsync();

                var groupedMatchRecords = from record in matchRecords
                                          where record.PlayerId.HasValue
                                          group record by record.PlayerId.Value into g
                                          select g;

                var stats = from p in players
                           join gmr in groupedMatchRecords on p.Id equals gmr.Key
                           select PlayerStatItemResponse.FromMatchRecordGrouping(p, gmr);

                return stats.OrderByDescending(s => s.MMR);
            }
        }
    }
}
