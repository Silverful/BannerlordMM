using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Clan;
using BL.API.Services.Regions.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Clans.Queries
{
    public static class GetClansList
    {
        public record Query(string RegionShortName) : IRequest<IEnumerable<ClanListItem>>;

        public class GetClansListHandler : IRequestHandler<Query, IEnumerable<ClanListItem>>
        {
            private readonly IRepository<Clan> _repository;
            private readonly IMediator _mediator;

            public GetClansListHandler(IRepository<Clan> repository, IMediator mediator)
            {
                _repository = repository;
                _mediator = mediator;
            }

            public async Task<IEnumerable<ClanListItem>> Handle(Query request, CancellationToken cancellationToken)
            {
                var region = await _mediator.Send(new GetRegionByShortName.Query(request.RegionShortName));

                return (await _repository.GetWhereAsync(c => c.RegionId == region.Id)).Select(c => new ClanListItem { ClanId = c.Id, Name = c.Name });
            }
        }

        public class ClanListItem
        {
            public Guid ClanId { get; set; }
            public string Name { get; set; }
        }
    }
}
