using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using BL.API.Services.Stats.Model;
using BL.API.Services.Stats.Utility;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetPlayerStats
    {
        public record Query(string PlayerId) : IRequest<PlayerStatItemResponse>;

        public class GetPlayerStatsHandler : IRequestHandler<Query, PlayerStatItemResponse>
        {
            private readonly IRepository<PlayerMatchRecord> _matchRecords;
            private readonly IRepository<Player> _players;


            public GetPlayerStatsHandler(IRepository<Player> players, IRepository<PlayerMatchRecord> matchRecords)
            {
                _matchRecords = matchRecords;
                _players = players;
            }

            public async Task<PlayerStatItemResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!Guid.TryParse(request.PlayerId, out Guid id)) throw new GuidCantBeParsedException();

                var player = await _players.GetByIdAsync(id);

                if (player == null) throw new NotFoundException();

                var matchRecords = await _matchRecords.GetWhereAsync(m => m.PlayerId == id);

                var stats = StatsQueryHelper.GetPlayerStats(player, matchRecords);

                return stats;
            }
        }
    }
}
