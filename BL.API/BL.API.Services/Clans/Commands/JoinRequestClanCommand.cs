using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Clan;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Clans.Commands
{
    public class JoinRequestClanCommand : IRequest<JoinRequestClanResponse>
    {
        public Guid RequestFromPlayerId { get; set; }
        public Guid RequestToClanId { get; set; }

        public class JoinRequestClanCommandHandler : IRequestHandler<JoinRequestClanCommand, JoinRequestClanResponse>
        {
            private readonly IRepository<Clan> _repository;
            private readonly IRepository<Player> _players;
            private readonly IRepository<ClanJoinRequest> _joinRequests;
            private readonly ILogger<JoinRequestClanCommandHandler> _logger;

            public JoinRequestClanCommandHandler(IRepository<Clan> repository,
                IRepository<Player> players,
                IRepository<ClanJoinRequest> joinRequests,
                ILogger<JoinRequestClanCommandHandler> logger)
            {
                _repository = repository;
                _joinRequests = joinRequests;
                _players = players;
                _logger = logger;
            }

            public async Task<JoinRequestClanResponse> Handle(JoinRequestClanCommand request, CancellationToken cancellationToken)
            {
                var requestingPlayer = await _players.GetByIdAsync(request.RequestFromPlayerId, false);

                if (requestingPlayer == null)
                {
                    throw new NotFoundException();
                }

                var clan = await _repository.GetByIdAsync(request.RequestToClanId, false, c => c.ClanMembers);

                if (clan == null)
                {
                    throw new NotFoundException();
                }

                string message = "";
                Guid? requestId = null;

                if (requestingPlayer.ClanMember != null)
                {
                    message = $"User {requestingPlayer.Nickname} ({requestingPlayer.Id.ToString()}) is already a member of a clan";

                    return new JoinRequestClanResponse { Message = message, RequestId = null };
                }

                if (clan.EnterType == ClanEnterType.Free)
                {
                    var requestingClanMember = new ClanMember()
                    {
                        ClanId = clan.Id,
                        PlayerId = requestingPlayer.Id,
                        MemberType = ClanMemberType.Soldier
                    };

                    clan.ClanMembers.Add(requestingClanMember);
                    await _repository.UpdateAsync(clan);
                    message = $"User {requestingPlayer.Nickname} ({requestingPlayer.Id.ToString()}) added to {clan.Name} ({clan.Id.ToString()})";
                }

                if (clan.EnterType == ClanEnterType.Closed)
                {
                    message = $"{clan.Name} ({clan.Id.ToString()}) is closed. User cannot be added via request.";
                }

                if (clan.EnterType == ClanEnterType.Request)
                {
                    var existingRequests = await _joinRequests.GetWhereAsync(r => r.FromPlayerId == requestingPlayer.Id
                        && !r.IsApproved
                        && !r.IsDismissed
                        && r.ToClan.RegionId == clan.RegionId);

                    if (existingRequests != null)
                    {
                        foreach (var req in existingRequests)
                        {
                            req.IsDismissed = true;
                        }

                        await _joinRequests.UpdateRangeAsync(existingRequests);
                    }

                    var joinRequest = new ClanJoinRequest()
                    {
                        FromPlayerId = requestingPlayer.Id,
                        ToClanId = clan.Id,
                        IsApproved = false,
                        IsDismissed = false
                    };

                    await _joinRequests.CreateAsync(joinRequest);

                    requestId = joinRequest.Id;
                    message = $"User {requestingPlayer.Nickname} ({requestingPlayer.Id.ToString()}) wants to join {clan.Name} ({clan.Id.ToString()}).";
                }

                _logger?.LogInformation(message);
                return new JoinRequestClanResponse { Message = message, RequestId = requestId }; 
            }
        }
    }
}
