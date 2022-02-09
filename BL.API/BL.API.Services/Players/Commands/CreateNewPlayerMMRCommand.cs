﻿using BL.API.Core.Domain.Player;
using BL.API.Services.MMR;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Commands
{
    public class CreateNewPlayerMMRCommand : IRequest<PlayerMMR>
    {
        public Guid SeasonId { get; set; }
        public Guid PlayerId { get; set; }


        public class StartNewSeasonCommandHandler : IRequestHandler<CreateNewPlayerMMRCommand, PlayerMMR>
        {
            private readonly BasicMMRCalculationProperties _mmrProps;

            public StartNewSeasonCommandHandler(BasicMMRCalculationProperties settings)
            {
                _mmrProps = settings;
            }

            private double StartMMR { get => _mmrProps.StartMMR; }

            public Task<PlayerMMR> Handle(CreateNewPlayerMMRCommand request, CancellationToken cancellationToken)
            {
                var newMMR = new PlayerMMR
                {
                    MMR = StartMMR,
                    SeasonId = request.SeasonId,
                    PlayerId = request.PlayerId
                };

                return Task.FromResult(newMMR);
            }
        }
    }
}
