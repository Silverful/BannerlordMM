using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Matches.Queries
{
    public static class GetPlayersCalibrationRecords
    {
        public record Query(Guid PlayerId, Guid SeasonId) : IRequest<IEnumerable<PlayerMatchRecord>>;

        public class GetPlayersCalibrationRecordsHandler : IRequestHandler<Query, IEnumerable<PlayerMatchRecord>>
        {
            private readonly IRepository<PlayerMatchRecord> _repository;

            public GetPlayersCalibrationRecordsHandler(IRepository<PlayerMatchRecord> repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<PlayerMatchRecord>> Handle(Query request, CancellationToken cancellationToken)
            {
                var records = (await _repository.GetWhereAsync(pr => pr.PlayerId == request.PlayerId && pr.Match.SeasonId == request.SeasonId)).OrderBy(pr => pr.CalibrationIndex);

                return records;
            }
        }
    }
}
