using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Commands
{
    public class UpdatePlayerCommand : IRequest<Task>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [StringLength(64)]
        public string Nickname { get; set; }
        [StringLength(32)]
        public string Country { get; set; }
        [StringLength(32)]
        public string Clan { get; set; }
        [StringLength(8)]
        public string MainClass { get; set; }
        [StringLength(8)]
        public string SecondaryClass { get; set; }
        [Required]
        public int DiscordId { get; set; }
        [Required]
        public int PlayerMMR { get; set; }

        public Player ToPlayer()
        {
            var mainClass = (PlayerClass)Enum.Parse(typeof(PlayerClass), this.MainClass);
            var secondaryClass = (PlayerClass)Enum.Parse(typeof(PlayerClass), this.SecondaryClass);

            return new Player
            {
                Id = this.Id,
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
