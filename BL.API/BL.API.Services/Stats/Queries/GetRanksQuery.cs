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
        public record Query(IEnumerable<Player> Players) : IRequest<IDictionary<string, double>>;

        public class GetRanksQueryHandler : IRequestHandler<Query, IDictionary<string, double>>
        {
            private readonly IRepository<Player> _players;

            public GetRanksQueryHandler(IRepository<Player> players)
            {
                _players = players;
            }


            public async Task<IDictionary<string, double>> Handle(Query request, CancellationToken cancellationToken)
            {
                var players = request.Players ?? (await _players.GetAllAsync());

                var maxRating = players.Max(x => x.PlayerMMR.MMR);
                var minRating = players.Min(x => x.PlayerMMR.MMR);

                var rankTable = StatsQueryHelper.RankMultipliers;

                var ranks = rankTable
                    .Select(rm => 
                    {
                        var rank = rm.Key;
                        double value = 0;

                        switch (rank)
                        {
                            case "Copper":
                                value = 2000;
                                break;
                            case "Wood":
                                value = minRating + rm.Value;
                                break;
                            default:
                                value = (maxRating - 2000)  * rm.Value + 2000;
                                break;
                        }
                        return new KeyValuePair<string, double>(rank, value);
                    })
                    .ToDictionary();

                return ranks;
            }
        }
    }
}
