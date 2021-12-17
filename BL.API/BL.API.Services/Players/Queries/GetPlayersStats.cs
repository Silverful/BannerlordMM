using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
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

            public GetPlayersStatsHandler(IRepository<PlayerMatchRecord> matchRecords)
            {
                _matchRecords = matchRecords;
            }

            public async Task<IEnumerable<PlayerStatItemResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var matchRecords = await _matchRecords.GetAllAsync();

                var stats =
                    from record in matchRecords
                    group record by record.PlayerId into g
                    select PlayerStatItemResponse.FromMatchRecordGrouping(g);

                return stats.OrderByDescending(s => s.MMR);
            }
        }
    }
}
