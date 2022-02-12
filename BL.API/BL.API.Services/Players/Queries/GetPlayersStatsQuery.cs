﻿using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
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
        public record Query(IEnumerable<Player> Players, IEnumerable<PlayerMatchRecord> MatchRecords, IDictionary<string, double> RankTable) : IRequest<IEnumerable<PlayerStatItemResponse>>;

        public class GetPlayersStatsHandler : IRequestHandler<Query, IEnumerable<PlayerStatItemResponse>>
        {
            private readonly IRepository<Match> _matches;
            private readonly IRepository<Player> _players;
            private readonly ISeasonResolverService _seasonResolver;
            private readonly IMediator _mediator;

            public GetPlayersStatsHandler(IRepository<Match> matches,
                IRepository<Player> players,
                ISeasonResolverService seasonResolver,
                IMediator mediator)
            {
                _matches = matches;
                _players = players;
                _seasonResolver = seasonResolver;
                _mediator = mediator;
            }

            public async Task<IEnumerable<PlayerStatItemResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var season = await _seasonResolver.GetCurrentSeasonAsync();

                var players = request.Players ?? await _players.GetAllAsync();
                var matchRecords = request.MatchRecords ?? (await _matches.GetWhereAsync(m => m.SeasonId == season.Id, true, m => m.PlayerRecords)).Select(x => x.PlayerRecords).SelectMany(x => x);
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
                    .OrderByDescending(s => s.Played >= 10 ? 1 : 0)
                    .ThenByDescending(s => s.MMR)
                    .Select((s, i) =>
                    {
                        s.Position = i + 1;
                        return s;
                    })
                    .ToList();

                return response;
            }
        }
    }
}
