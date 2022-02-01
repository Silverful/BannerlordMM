using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
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

        public class StartNewSeasonCommandHandler : IRequestHandler<StartNewSeasonCommand, Guid>
        {
            private readonly IRepository<Season> _seasons;
            private readonly IRepository<PlayerMMR> _mmrs;

            public StartNewSeasonCommandHandler(IRepository<Season> seasons, 
                IRepository<PlayerMMR> mmrs)
            {
                _seasons = seasons;
                _mmrs = mmrs;
            }

            public async Task<Guid> Handle(StartNewSeasonCommand request, CancellationToken cancellationToken)
            {
                var currentSeason = await _seasons.GetFirstWhereAsync(s => s.OnGoing && !s.IsTestingSeason, false);

                currentSeason.OnGoing = false;
                currentSeason.Finished = DateTime.UtcNow;

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _seasons.UpdateAsync(currentSeason);

                    var newSeason = new Season
                    {
                        Title = request.SeasonName,
                        OnGoing = true,
                        IsTestingSeason = false,
                        Created = DateTime.UtcNow
                    };

                    await _seasons.CreateAsync(newSeason);

                    var mmrs = await _mmrs.GetAllAsync();

                    foreach (var mmr in mmrs)
                    {
                        var newMMR = new PlayerMMR
                        {
                            MMR = 0,
                            SeasonId = newSeason.Id,
                            PlayerId = mmr.PlayerId
                        };

                        await _mmrs.CreateAsync(newMMR);
                    }

                    return newSeason.Id;
                }
            }
        }
    }
}
