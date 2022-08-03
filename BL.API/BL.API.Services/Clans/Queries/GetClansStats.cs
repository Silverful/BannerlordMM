using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Clans.Queries
{
    public static class GetClansStats
    {
        public record Query() : IRequest<IEnumerable<ClanStatsItem>>;

        public class GetClansStatsHandler : IRequestHandler<Query, IEnumerable<ClanStatsItem>>
        {
            public Task<IEnumerable<ClanStatsItem>> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
