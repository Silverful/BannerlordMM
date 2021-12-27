using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BL.API.Services.Players.Commands
{
    public class AddPlayerCommand : IRequest<Guid>
    {
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

        public Player ToPlayer()
        {
            var mainClass = (PlayerClass)Enum.Parse(typeof(PlayerClass), this.MainClass);
            var secondaryClass = (PlayerClass)Enum.Parse(typeof(PlayerClass), this.SecondaryClass);

            return new Player
            {
                Nickname = this.Nickname,
                Country = this.Country,
                Clan = this.Clan,
                IsIGL = this.IGL,
                MainClass = mainClass,
                SecondaryClass = secondaryClass,
                DiscordId = this.DiscordId
            };
        }

        public class AddPlayerCommandHandler : IRequestHandler<AddPlayerCommand, Guid>
        {
            private readonly IRepository<Player> _repository;
            private readonly IRepository<PlayerMMR> _mmr;
            private readonly ILogger<AddPlayerCommandHandler> _logger;
            private readonly ISeasonResolverService _seasonService;

            public AddPlayerCommandHandler(IRepository<Player> repository,
                IRepository<PlayerMMR> mmr,
                ISeasonResolverService seasonService,
                ILogger<AddPlayerCommandHandler> logger)
            {
                _repository = repository;
                _mmr = mmr;
                _seasonService = seasonService;
                _logger = logger;
            }

            public async Task<Guid> Handle(AddPlayerCommand request, CancellationToken cancellationToken)
            {
                if (request.DiscordId.HasValue)
                {
                    if (await _repository.GetFirstWhereAsync(p => p.DiscordId == request.DiscordId) != null) throw new AlreadyExistsException();
                }
                else
                {
                    if (await _repository.GetFirstWhereAsync(p => p.Nickname == request.Nickname) != null) throw new AlreadyExistsException();
                }

                var currentSeason = await _seasonService.GetCurrentSeasonAsync();

                var player = request.ToPlayer();

                var playerMMRs = player.PlayerMMRs ?? new List<PlayerMMR>();
                playerMMRs.Add(new PlayerMMR
                {
                    SeasonId = currentSeason.Id,
                    MMR = 0
                });

                player.PlayerMMRs = playerMMRs;
                    
                await _repository.CreateAsync(player);

                _logger?.LogInformation($"Player created {JsonSerializer.Serialize(player, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve })}");

                return player.Id;
            }
        }
    }
}
