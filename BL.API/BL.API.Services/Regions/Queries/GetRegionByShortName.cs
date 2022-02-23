using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Regions.Queries
{
    public class GetRegionByShortName
    {
        public record Query(string ShortName) : IRequest<Region>;

        public class GetRegionByShortNameHandler : IRequestHandler<Query, Region>
        {
            private readonly IRepository<Region> _regions;

            public GetRegionByShortNameHandler(IRepository<Region> regions)
            {
                _regions = regions;
            }

            public async Task<Region> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _regions.GetFirstWhereAsync(r => r.ShortName == request.ShortName);
            }
        }
    }
}
