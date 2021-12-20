using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Services.Stats.Model;
using BL.API.Services.Stats.Utility;
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

                var stats = StatsQueryHelper.GetPlayersStats(players, matchRecords);

                return stats.OrderByDescending(s => s.MMR);
            }
        }
    }
}
