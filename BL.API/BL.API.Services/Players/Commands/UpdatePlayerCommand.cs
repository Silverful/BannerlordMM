using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        public bool IGL { get; set; }

        public Player ToPlayer(Player player)
        {
            var mainClass = (PlayerClass)Enum.Parse(typeof(PlayerClass), this.MainClass);
            var secondaryClass = (PlayerClass)Enum.Parse(typeof(PlayerClass), this.SecondaryClass);

            player.Nickname = this.Nickname;
            player.Country = this.Country;
            player.IsIGL = this.IGL;
            player.Clan = this.Clan;
            player.MainClass = mainClass;
            player.SecondaryClass = secondaryClass;
            player.DiscordId = this.DiscordId;

            return player;
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

                var currentPlayer = await _repository.GetByIdAsync(id, false);

                if (currentPlayer == null) throw new NotFoundException();

                if (currentPlayer.DiscordId != request.DiscordId
                    && await _repository.GetFirstWhereAsync(p => p.DiscordId == request.DiscordId) != null)
                {
                    throw new AlreadyExistsException();
                }

                if (currentPlayer.Nickname.ToUpper() != request.Nickname.ToUpper() 
                    && await _repository.GetFirstWhereAsync(p => p.Nickname.ToUpper() == request.Nickname.ToUpper()) != null)
                {
                    throw new AlreadyExistsException();
                }

                var player = request.ToPlayer(currentPlayer);

                await _repository.UpdateAsync(player);

                _logger?.LogInformation($"Player updated {JsonSerializer.Serialize(player, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve })}");
                return Task.CompletedTask;
            }
        }
    }
}
