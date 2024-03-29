﻿using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using BL.API.Services.Regions.Queries;
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
        public string RegionShortName { get; set; }

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
            private readonly ILogger<AddPlayerCommandHandler> _logger;
            private readonly ISeasonResolverService _seasonService;
            private readonly IMediator _mediator;

            public AddPlayerCommandHandler(IRepository<Player> repository,
                ISeasonResolverService seasonService,
                IMediator mediator,
                ILogger<AddPlayerCommandHandler> logger)
            {
                _repository = repository;
                _mediator = mediator;
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
                    var normalizedNickname = request.Nickname.ToUpper();
                    if (await _repository.GetFirstWhereAsync(p => p.Nickname.ToUpper() == normalizedNickname) != null) throw new AlreadyExistsException();
                }

                var region = await _mediator.Send(new GetRegionByShortName.Query(request.RegionShortName));
                var currentSeason = await _seasonService.GetCurrentSeasonAsync(region.Id);

                var player = request.ToPlayer();

                await _repository.CreateAsync(player);

                _logger?.LogInformation($"Player created {JsonSerializer.Serialize(player, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve })}");

                return player.Id;
            }
        }
    }
}
