using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Clan;
using BL.API.Core.Exceptions;
using BL.API.Services.Regions.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Clans.Queries
{
    public static class GetPendingRequests
    {
        public record Query(string RegionShortName, string ClanId) : IRequest<IEnumerable<PendingRequestsResponseItem>>;

        public class GetPendingRequestsHandler : IRequestHandler<Query, IEnumerable<PendingRequestsResponseItem>>
        {
            private readonly IRepository<ClanJoinRequest> _repository;
            private readonly IMediator _mediator;

            public GetPendingRequestsHandler(IRepository<ClanJoinRequest> repository, IMediator mediator)
            {
                _repository = repository;
                _mediator = mediator;
            }

            public async Task<IEnumerable<PendingRequestsResponseItem>> Handle(Query request, CancellationToken cancellationToken)
            {
                var region = await _mediator.Send(new GetRegionByShortName.Query(request.RegionShortName));

                if (!Guid.TryParse(request.ClanId, out Guid id)) throw new GuidCantBeParsedException();

                var requests = await _repository.GetWhereAsync(r => r.ToClan.RegionId == region.Id && !r.IsApproved && !r.IsDismissed && r.ToClanId == id);

                return requests.Select(r => new PendingRequestsResponseItem { PlayerId = r.FromPlayerId, Nickname = r.FromPlayer.Nickname, Created = r.Created });
            }
        }

        public class PendingRequestsResponseItem
        {
            public Guid PlayerId { get; set; }
            public string Nickname { get; set; }
            public DateTime Created { get; set; }
        }

    }
}
