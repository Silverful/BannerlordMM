using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BL.API.Services.Seasons.Commands
{
    public class DeleteCurrentSeasonCommand : IRequest<Task>
    {
        public class DeleteCurrentSeasonCommandHandler : IRequestHandler<DeleteCurrentSeasonCommand, Task>
        {
            private readonly IRepository<Season> _seasons;
            private readonly IRepository<PlayerMMR> _mmrs;

            public DeleteCurrentSeasonCommandHandler(IRepository<Season> seasons,
                IRepository<PlayerMMR> mmrs)
            {
                _seasons = seasons;
                _mmrs = mmrs;
            }

            public async Task<Task> Handle(DeleteCurrentSeasonCommand request, CancellationToken cancellationToken)
            {
                var currentSeason = await _seasons.GetFirstWhereAsync(s => s.OnGoing, false);
                var previousSeason = (await _seasons.GetWhereAsync(s => !s.OnGoing, false)).OrderByDescending(s => s.Index).First();

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var currentMMRs = (await _mmrs.GetWhereAsync(m => m.SeasonId == currentSeason.Id, false)).ToList();

                    foreach (var currentMMR in currentMMRs)
                    {
                        await _mmrs.DeleteAsync(currentMMR);
                    }

                    await _seasons.DeleteAsync(currentSeason.Id);

                    previousSeason.OnGoing = true;
                    previousSeason.Finished = null;

                    await _seasons.UpdateAsync(previousSeason);

                    scope.Complete();
                }
                return Task.CompletedTask;
            }
        }
    }
}
