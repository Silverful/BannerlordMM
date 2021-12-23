using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Commands
{
    public class UpdatePlayerCommand : IRequest<Task>
    {
        [Required]
        public string Id { get; set; }
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
        public long? DiscordId { get; set; }
        [Required]
        public int PlayerMMR { get; set; }
        public bool IGL { get; set; }

        public Player ToPlayer(Guid id)
        {
            var mainClass = (PlayerClass)Enum.Parse(typeof(PlayerClass), this.MainClass);
            var secondaryClass = (PlayerClass)Enum.Parse(typeof(PlayerClass), this.SecondaryClass);

            return new Player
            {
                Id = id,
                Nickname = this.Nickname,
                Country = this.Country,
                IsIGL = this.IGL,
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
            private readonly ILogger<UpdatePlayerCommandHandler> _logger;

            public UpdatePlayerCommandHandler(IRepository<Player> repository, ILogger<UpdatePlayerCommandHandler> logger)
            {
                _repository = repository;
                _logger = logger;
            }

            public async Task<Task> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
            {
                if (!Guid.TryParse(request.Id, out Guid id)) throw new GuidCantBeParsedException();

                var player = request.ToPlayer(id);

                var currentPlayer = await _repository.GetByIdAsync(id);

                if (currentPlayer == null) throw new NotFoundException();

                await _repository.UpdateAsync(player);

                _logger?.LogInformation($"Player updated {JsonSerializer.Serialize(player)}");
                return Task.CompletedTask;
            }
        }
    }
}
