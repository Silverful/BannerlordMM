using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Commands
{
    public class AddPlayerCommand : IRequest<Guid>
    {
        public string Nickname { get; set; }
        public string Country { get; set; }
        public string Clan { get; set; }
        public string MainClass { get; set; }
        public string SecondaryClass { get; set; }
        public int DiscordId { get; set; }

        public Player ToPlayer()
        {
            var mainClass = (PlayerClass)Enum.Parse(typeof(PlayerClass), this.MainClass);
            var secondaryClass = (PlayerClass)Enum.Parse(typeof(PlayerClass), this.SecondaryClass);

            return new Player()
            {
                Nickname = this.Nickname,
                Country = this.Country,
                Clan = this.Clan,
                MainClass = mainClass,
                SecondaryClass = secondaryClass,
                DiscordId = this.DiscordId,
                PlayerMMR = 0
            };
        }

        public class AddPlayerCommandHandler : IRequestHandler<AddPlayerCommand, Guid>
        {
            private readonly IRepository<Player> _repository;

            public AddPlayerCommandHandler(IRepository<Player> repository)
            {
                _repository = repository;
            }

            public async Task<Guid> Handle(AddPlayerCommand request, CancellationToken cancellationToken)
            {
                var player = request.ToPlayer();

                await _repository.CreateAsync(player);

                return player.Id;
            }
        }
    }
}
