using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Clan;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Clans
{
    public class CreateClanCommand : IRequest<Guid>
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; } 
        public string Color { get; set; }
        public string AvatarURL { get; set; }
        public Guid LeaderId { get; set; }
        public ClanEnterType EnterType { get; set; }


        public class CreateClanCommandHandler : IRequestHandler<CreateClanCommand, Guid>
        {
            private readonly IRepository<Clan> _repository;
            private readonly IRepository<Player> _players;
            private readonly ILogger<CreateClanCommandHandler> _logger;

            public CreateClanCommandHandler(IRepository<Clan> repository,
                IRepository<Player> players,
                ILogger<CreateClanCommandHandler> logger)
            {
                _repository = repository;
                _players = players;
                _logger = logger;
            }

            public async Task<Guid> Handle(CreateClanCommand request, CancellationToken cancellationToken)
            {
                var clanLeader = _players.GetFirstWhereAsync(p => p.Id == request.LeaderId);

                if (clanLeader == null)
                {
                    throw new NotFoundException();
                }

                if (await _repository.GetFirstWhereAsync(c => c.Leader.Id == request.LeaderId || c.Name == request.Name) != null)
                {
                    throw new AlreadyExistsException();
                }


                var clan = new Clan
                {
                    Name = request.Name,
                    Description = request.Description, 
                    Color = request.Color,
                    AvatarURL = request.AvatarURL,

                }
            }
        }
    }
}
