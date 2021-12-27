using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using BL.API.Services.Extensions;
using BL.API.Services.Stats.Utility;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetRanksQuery
    {
        public record Query(IEnumerable<Player> Players) : IRequest<IDictionary<string, decimal>>;

        public class GetRanksQueryHandler : IRequestHandler<Query, IDictionary<string, decimal>>
        {
            private readonly IRepository<Player> _players;

            public GetRanksQueryHandler(IRepository<Player> players)
            {
                _players = players;
            }


            public async Task<IDictionary<string, decimal>> Handle(Query request, CancellationToken cancellationToken)
            {
                var players = request.Players ?? (await _players.GetAllAsync());

                var maxRating = players.Max(x => x.PlayerMMR.MMR);
                var minRating = players.Min(x => x.PlayerMMR.MMR);

                var rankTable = StatsQueryHelper.RankMultipliers;

                var ranks = rankTable
                    .Select(rm => new KeyValuePair<string, decimal>(rm.Key, rm.Key == "Wood" ? minRating + rm.Value : maxRating * rm.Value))
                    .ToDictionary();

                return ranks;
            }
        }
    }
}
