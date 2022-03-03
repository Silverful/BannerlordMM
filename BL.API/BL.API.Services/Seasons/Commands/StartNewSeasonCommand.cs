using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Domain.Settings;
using BL.API.Services.Players.Commands;
using BL.API.Services.Regions.Queries;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BL.API.Services.Seasons.Commands
{
    public class StartNewSeasonCommand : IRequest<Guid>
    {
        [Required]
        public string SeasonName { get; set; }
        [Required]
        public string RegionShortName { get; set; }

        public class StartNewSeasonCommandHandler : IRequestHandler<StartNewSeasonCommand, Guid>
        {
            private readonly IRepository<Season> _seasons;
            private readonly IMediator _mediator;

            public StartNewSeasonCommandHandler(IRepository<Season> seasons,
                IMediator mediator)
            {
                _mediator = mediator;
                _seasons = seasons;
            }

            public async Task<Guid> Handle(StartNewSeasonCommand request, CancellationToken cancellationToken)
            {
                var region = await _mediator.Send(new GetRegionByShortName.Query(request.RegionShortName));
                var currentSeason = await _seasons.GetFirstWhereAsync(s => s.RegionId == region.Id && s.OnGoing && !s.IsTestingSeason, false);

                var newSeason = new Season
                {
                    Title = request.SeasonName,
                    OnGoing = true,
                    IsTestingSeason = false,
                    Started = DateTime.UtcNow,
                    RegionId = region.Id
                };

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (currentSeason != null)
                    {
                        currentSeason.OnGoing = false;
                        currentSeason.Finished = DateTime.UtcNow;
                        await _seasons.UpdateAsync(currentSeason);
                    }

                    await _seasons.CreateAsync(newSeason);

                    scope.Complete();
                }

                return newSeason.Id;
            }
        }
    }
}
