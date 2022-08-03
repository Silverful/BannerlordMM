using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Clan;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Clans.Commands
{
    public class ApproveRequestCommand : IRequest<Task>
    {
        [Required]
        public Guid ApprovedByPlayerId { get; set; }
        [Required]
        public Guid RequestId { get; set; }
        [Required]
        public bool IsApproved { get; set; }

        public class ApproveRequestCommandHandler : IRequestHandler<ApproveRequestCommand, Task>
        {
            private readonly IRepository<Clan> _repository;
            private readonly IRepository<Player> _players;
            private readonly IRepository<Clan> _clans;
            private readonly IRepository<ClanJoinRequest> _requests;
            private readonly ILogger<ApproveRequestCommandHandler> _logger;

            public ApproveRequestCommandHandler(IRepository<Clan> repository,
                IRepository<Player> players,
                IRepository<Clan> clans,
                IRepository<ClanJoinRequest> requests,
                ILogger<ApproveRequestCommandHandler> logger)
            {
                _repository = repository;
                _clans = clans;
                _players = players;
                _logger = logger;
            }

            public async Task<Task> Handle(ApproveRequestCommand request, CancellationToken cancellationToken)
            {
                var approvingPlayer = await _players.GetByIdAsync(request.ApprovedByPlayerId);

                if (approvingPlayer == null)
                {
                    throw new NotFoundException();
                }

                var joinRequest = await _requests.GetByIdAsync(request.RequestId);

                if (joinRequest == null)
                {
                    throw new NotFoundException();
                }

                if (approvingPlayer.ClanMember.MemberType == ClanMemberType.Soldier)
                {
                    throw new InsufficientPermissions();
                }

                if (approvingPlayer.ClanMember.ClanId != joinRequest.ToClanId)
                {
                    throw new NotFoundException();
                }

                var clan = await _clans.GetByIdAsync(joinRequest.ToClanId);

                if (clan == null)
                {
                    throw new NotFoundException();
                }
                
                if (clan.ClanMembers.Where(cm => cm.PlayerId == joinRequest.FromPlayerId).FirstOrDefault() != null)
                {
                    throw new AlreadyExistsException();
                }

                var joiningPlayer = await _players.GetByIdAsync(joinRequest.FromPlayerId);

                joinRequest.ApprovedTimestamp = DateTime.UtcNow;
                joinRequest.IsApproved = request.IsApproved;

                await _requests.UpdateAsync(joinRequest);

                if (request.IsApproved)
                {
                    var newClanMember = new ClanMember
                    {
                        ClanId = clan.Id,
                        PlayerId = joiningPlayer.Id,
                        MemberType = ClanMemberType.Soldier
                    };

                    clan.ClanMembers.Add(newClanMember);
                }

                return Task.CompletedTask;
            }
        }
    }
}
