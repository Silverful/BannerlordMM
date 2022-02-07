using BL.API.Core.Abstractions.Repositories;
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
        public record Query(Guid PlayerId, IEnumerable<PlayerMatchRecord> Records) : IRequest<double>;

        public class GetPlayersAvgCalibrationScoreQueryHandler : IRequestHandler<Query, double>
        {
            private readonly IRepository<PlayerMatchRecord> _repository;

            public GetPlayersAvgCalibrationScoreQueryHandler(IRepository<PlayerMatchRecord> repository)
            {
                _repository = repository;
            }

            public async Task<double> Handle(Query request, CancellationToken cancellationToken)
            {
                var records = request.Records ?? await _repository.GetWhereAsync(x => x.PlayerId == request.PlayerId);

                var roundsPlayed = records.Sum(x => x.Match.RoundsPlayed);

                var avgScore = roundsPlayed == 0 ? 0 : (double)records.Sum(x => x.Score) / roundsPlayed;

                return avgScore;
            }
        }
    }
}
