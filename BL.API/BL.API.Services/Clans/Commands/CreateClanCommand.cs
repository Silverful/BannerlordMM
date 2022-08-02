using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Clan;
using BL.API.Core.Domain.Player;
using BL.API.Core.Domain.Settings;
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
    public class CreateClanCommand : IRequest<Guid>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string RegionShortName { get; set; }
        public string Description { get; set; } 
        public string Color { get; set; }
        public string AvatarURL { get; set; }
        public Guid LeaderId { get; set; }
        public ClanEnterType EnterType { get; set; }

        public class CreateClanCommandHandler : IRequestHandler<CreateClanCommand, Guid>
        {
            private readonly IRepository<Clan> _repository;
            private readonly IRepository<Region> _regionRepository;
            private readonly IRepository<Player> _players;
            private readonly ILogger<CreateClanCommandHandler> _logger;

            public CreateClanCommandHandler(IRepository<Clan> repository,
                IRepository<Region> regionRepository,
                IRepository<Player> players,
                ILogger<CreateClanCommandHandler> logger)
            {
                _repository = repository;
                _regionRepository = regionRepository;
                _players = players;
                _logger = logger;
            }

            public async Task<Guid> Handle(CreateClanCommand request, CancellationToken cancellationToken)
            {
                var region = await _regionRepository.GetFirstWhereAsync(r => r.ShortName == request.RegionShortName);

                if (region == null)
                {
                    throw new NotFoundException();
                }

                var clanLeader = await _players.GetFirstWhereAsync(p => p.Id == request.LeaderId);

                if (clanLeader == null)
                {
                    throw new NotFoundException();
                }

                if (await _repository.GetFirstWhereAsync(c => c.ClanMembers.FirstOrDefault(cm => cm.MemberType == ClanMemberType.Leader).PlayerId == request.LeaderId || c.Name == request.Name) != null)
                {
                    throw new AlreadyExistsException();
                }

                //if (await _repository.GetFirstWhereAsync(c => c.GetLeader().PlayerId == request.LeaderId || c.Name == request.Name) != null)
                //{
                //    throw new AlreadyExistsException();
                //}

                //TODO ADD NAME UNIQUE CHECK

                var members = new List<ClanMember>();
                members.Add(new ClanMember
                {
                    PlayerId = request.LeaderId,
                    MemberType = ClanMemberType.Leader
                });

                var clan = new Clan
                {
                    Name = request.Name,
                    Description = request.Description,
                    Color = request.Color,
                    RegionId = region.Id,
                    AvatarURL = request.AvatarURL,
                    ClanMembers = members,
                    EnterType = request.EnterType
                };

                await _repository.CreateAsync(clan);

                _logger?.LogInformationSerialized("Clan created", clan);

                return clan.Id;
            }
        }
    }
}
