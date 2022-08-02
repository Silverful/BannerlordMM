using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Clan;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using BL.API.Services.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Clans.Commands
{
    public class UpdateClanCommand : IRequest<Task>
    {
        [Required]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string AvatarURL { get; set; }
        [Required]
        public Guid LeaderId { get; set; }
        [Required]
        public ClanEnterType EnterType { get; set; }

        public class UpdateClanCommandHandler : IRequestHandler<UpdateClanCommand, Task>
        {
            private readonly IRepository<Clan> _repository;
            private readonly IRepository<Player> _players;
            private readonly ILogger<UpdateClanCommandHandler> _logger;

            public UpdateClanCommandHandler(IRepository<Clan> repository,
                IRepository<Player> players,
                ILogger<UpdateClanCommandHandler> logger)
            {
                _repository = repository;
                _players = players;
                _logger = logger;
            }

            public async Task<Task> Handle(UpdateClanCommand request, CancellationToken cancellationToken)
            {
                var clanLeader = await _players.GetByIdAsync(request.LeaderId, false);

                if (clanLeader == null) throw new NotFoundException();

                var clan = await _repository.GetByIdAsync(request.Id, false);

                if (clan == null) throw new NotFoundException();

                var members = (clan.ClanMembers ?? new List<ClanMember>()).ToList();

                ClanMember clanLeaderMember;

                clanLeaderMember = members.FirstOrDefault(m => m.MemberType == ClanMemberType.Leader);
                if (clanLeaderMember != null)
                {
                    members.Remove(clanLeaderMember);
                }

                clanLeaderMember = new ClanMember
                {
                    PlayerId = request.LeaderId,
                    MemberType = ClanMemberType.Leader
                };

                members.Add(clanLeaderMember);


                clan.Name = request.Name;
                clan.Description = request.Description;
                clan.AvatarURL = request.AvatarURL;
                clan.Color = request.Color;
                clan.ClanMembers = members;
                clan.EnterType = request.EnterType;

                //TODO ADD NAME UNIQUE CHECK
                await _repository.UpdateAsync(clan);

                _logger?.LogInformationSerialized($"Clan updated", clan);
                return Task.CompletedTask;
            }
        }
    }
}
