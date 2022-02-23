using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetPlayersAvgCalibrationScoreQuery
    {
        public record Query(Guid PlayerId, IEnumerable<PlayerMatchRecord> Records, Guid RegionId) : IRequest<double>;

        public class GetPlayersAvgCalibrationScoreQueryHandler : IRequestHandler<Query, double>
        {
            private readonly IRepository<PlayerMatchRecord> _repository;
            private readonly ISeasonResolverService _seasonResolver;

            public GetPlayersAvgCalibrationScoreQueryHandler(IRepository<PlayerMatchRecord> repository, ISeasonResolverService seasonResolver)
            {
                _repository = repository;
                _seasonResolver = seasonResolver;
            }

            public async Task<double> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentSeason = await _seasonResolver.GetCurrentSeasonAsync();

                var records = (request.Records ?? await _repository.GetWhereAsync(x => 
                    x.PlayerId == request.PlayerId 
                    && x.Match.SeasonId == currentSeason.Id 
                    && x.Match.RegionId == request.RegionId, 
                    true, 
                    x => x.Match))
                    .OrderByDescending(r => r.CalibrationIndex)
                    .ThenByDescending(r => r.Match.MatchDate)
                    .ThenByDescending(r => r.Match.Created)
                    .Take(10);

                var roundsPlayed = records.Sum(x => x.Match.RoundsPlayed);

                var avgScore = roundsPlayed == 0 ? 0 : (double)records.Sum(x => x.Score) / roundsPlayed;

                return avgScore;
            }
        }
    }
}
