using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Services.Extensions;
using BL.API.Services.MMR;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetRanksQuery
    {
        public record Query(IEnumerable<Player> Players, Guid regionId) : IRequest<IDictionary<string, double>>;

        public class GetRanksQueryHandler : IRequestHandler<Query, IDictionary<string, double>>
        {
            private readonly IRepository<Match> _matches;
            private readonly ISeasonResolverService _seasonResolver;
            private readonly List<RankProperty> _ranks;
            private readonly double _startingMMR;

            public GetRanksQueryHandler(IRepository<Match> matches, IOptions<BasicMMRCalculationProperties> options, ISeasonResolverService seasonResolver,
                IOptions<RanksConfig> ranks)
            {
                _matches = matches;
                _ranks = ranks.Value.Ranks;
                _seasonResolver = seasonResolver;
                _startingMMR = options.Value.StartMMR;
            }

            public async Task<IDictionary<string, double>> Handle(Query request, CancellationToken cancellationToken)
            {
                var season = await _seasonResolver.GetCurrentSeasonAsync(request.regionId);

                var players = request.Players ?? ((await _matches.GetWhereAsync(m => m.SeasonId == season.Id && m.RegionId == request.regionId, true, m => m.PlayerRecords))
                    .Select(x => x.PlayerRecords)
                    .SelectMany(x => x)
                    .Where(x => x.PlayerId != null) 
                    .GroupBy(x => x.PlayerId)
                    .Select(x => x.First()?.Player))
                    .Where(x => x.PlayerMMRs != null && x.PlayerMMRs.Count > 0);

                var maxRating = players != null && players.Count() > 0 ? players.Max(x => x?.GetPlayerMMR(request.regionId)?.MMR) ?? _startingMMR : _startingMMR;
                var minRating = players != null && players.Count() > 0 ? players.Min(x => x?.GetPlayerMMR(request.regionId)?.MMR) ?? _startingMMR : _startingMMR;

                var ranks = _ranks
                    .OrderBy(x => x.Position)
                    .Select(rm => 
                    {
                        var rank = rm.Title;
                        double value = 0;

                        if (rm.IsBottomRank && rm.Threshold != null)
                        {
                            var bottomValue = minRating + rm.Threshold.Value;
                            value = bottomValue >= _startingMMR ? _startingMMR - 1 : bottomValue;
                        }
                        else if (rm.Threshold != null)
                        {
                            value = rm.Threshold.Value;
                        }
                        else
                        {
                            value = (maxRating - _startingMMR) * rm.Percentile.Value + _startingMMR;
                        }

                        return new KeyValuePair<string, double>(rank, value);
                    })
                    .ToDictionary();

                return ranks;
            }
        }
    }
}
