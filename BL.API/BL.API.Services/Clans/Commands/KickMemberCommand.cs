using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Clan;
using BL.API.Core.Exceptions;
using BL.API.Services.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Clans.Commands
{
    public class KickMemberCommand
    {
        public record Query(Guid ClanId, Guid MemberId) : IRequest<Task>;

        public class KickMemberCommandHandler : IRequestHandler<Query, Task>
        {
            private readonly IRepository<Clan> _repository;
            private readonly ILogger<KickMemberCommandHandler> _logger;

            public KickMemberCommandHandler(IRepository<Clan> repository,
                ILogger<KickMemberCommandHandler> logger)
            {
                _repository = repository;
                _logger = logger;
            }

            public async Task<Task> Handle(Query request, CancellationToken cancellationToken)
            {
                var clan = await _repository.GetByIdAsync(request.ClanId, false);

                if (clan == null) throw new NotFoundException();

                var members = clan.ClanMembers;
                var memberToRemove = members.FirstOrDefault(m => m.PlayerId == request.MemberId);

                if (memberToRemove.MemberType == ClanMemberType.Leader)
                {
                    throw new InsufficientPermissions();
                }

                members.Remove(memberToRemove);

                await _repository.UpdateAsync(clan);

                _logger?.LogInformationSerialized($"Member kicked", memberToRemove);

                return Task.CompletedTask;
            }
        }
    }
}
