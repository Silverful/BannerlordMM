﻿using BL.API.Core.Abstractions.Repositories;
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
    public static class GetPlayersStats
    {
        public record Query(IEnumerable<Player> Players, IEnumerable<PlayerMatchRecord> MatchRecords, IDictionary<string, decimal> RankTable) : IRequest<IEnumerable<PlayerStatItemResponse>>;

        public class GetPlayersStatsHandler : IRequestHandler<Query, IEnumerable<PlayerStatItemResponse>>
        {
            private readonly IRepository<PlayerMatchRecord> _matchRecords;
            private readonly IRepository<Player> _players;
            private readonly IMediator _mediator;

            public GetPlayersStatsHandler(IRepository<PlayerMatchRecord> matchRecords, 
                IRepository<Player> players,
                IMediator mediator)
            {
                _matchRecords = matchRecords;
                _players = players;
                _mediator = mediator;
            }

            public async Task<IEnumerable<PlayerStatItemResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var players = request.Players ?? await _players.GetAllAsync();
                var matchRecords = request.MatchRecords ?? await _matchRecords.GetAllAsync();
                var rankTable = request.RankTable ?? await _mediator.Send(new GetRanksQuery.Query(players));

                var stats = from record in matchRecords
                                          where record.PlayerId.HasValue
                                          group record by record.PlayerId.Value into g
                                          join p in players on g.Key equals p.Id
                                          select PlayerStatItemResponse.FromMatchRecordGrouping(p, g, rankTable);

                return stats.OrderByDescending(s => s.MMR);
            }
        }
    }
}
