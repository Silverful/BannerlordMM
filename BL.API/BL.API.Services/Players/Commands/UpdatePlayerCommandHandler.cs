using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Commands
{
    public class UpdatePlayerCommand : IRequest<Task>
    {
        public string Nickname { get; set; }
        public string Country { get; set; }
        public string Clan { get; set; }
        public string MainClass { get; set; }
        public string SecondaryClass { get; set; }
        public int DiscordId { get; set; }
        public int PlayerMMR { get; set; }

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
                PlayerMMR = this.PlayerMMR
            };
        }

        public class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand, Task>
        {
            private readonly IRepository<Player> _repository;

            public UpdatePlayerCommandHandler(IRepository<Player> repository)
            {
                _repository = repository;
            }

            public async Task<Task> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
            {
                var player = request.ToPlayer();

                await _repository.UpdateAsync(player);
                return Task.CompletedTask;
            }
        }
    }
}
