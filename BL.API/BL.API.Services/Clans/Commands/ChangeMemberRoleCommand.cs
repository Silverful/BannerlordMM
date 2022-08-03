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
    public class ChangeMemberRoleCommand : IRequest<Task>
    {
        public Guid ClanId { get; set; }
        public Guid MemberId { get; set; }
        public ClanMemberType TargetRole { get; set; }

        public class ChangeMemberRoleCommandHandler : IRequestHandler<ChangeMemberRoleCommand, Task>
        {
            private readonly IRepository<Clan> _repository;
            private readonly ILogger<ChangeMemberRoleCommand> _logger;

            public ChangeMemberRoleCommandHandler(IRepository<Clan> repository,
                ILogger<ChangeMemberRoleCommand> logger)
            {
                _repository = repository;
                _logger = logger;
            }

            public async Task<Task> Handle(ChangeMemberRoleCommand request, CancellationToken cancellationToken)
            {
                var clan = await _repository.GetByIdAsync(request.ClanId, false);

                if (clan == null) throw new NotFoundException();

                var member = clan.ClanMembers.FirstOrDefault(m => m.PlayerId == request.MemberId);

                if (member == null)
                {
                    throw new NotFoundException();
                }

                if (request.TargetRole == ClanMemberType.Leader)
                {
                    var currentLeader = clan.ClanMembers.FirstOrDefault(m => m.MemberType == ClanMemberType.Leader);
                    currentLeader.MemberType = ClanMemberType.Soldier;
                }

                member.MemberType = request.TargetRole;
                await _repository.UpdateAsync(clan);

                _logger?.LogInformationSerialized($"Member role changed", member);

                return Task.CompletedTask;
            }
        }
    }
}
