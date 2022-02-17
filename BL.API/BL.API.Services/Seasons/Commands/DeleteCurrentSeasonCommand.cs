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
            private readonly IRepository<Match> _matches;
            private readonly IRepository<PlayerMatchRecord> _records;

            public DeleteCurrentSeasonCommandHandler(IRepository<Season> seasons,
                IRepository<Match> matches,
                IRepository<PlayerMatchRecord> records,
                IRepository<PlayerMMR> mmrs)
            {
                _seasons = seasons;
                _records = records;
                _matches = matches;
                _mmrs = mmrs;
            }

            public async Task<Task> Handle(DeleteCurrentSeasonCommand request, CancellationToken cancellationToken)
            {
                var currentSeason = await _seasons.GetFirstWhereAsync(s => s.OnGoing, true);
                var previousSeason = (await _seasons.GetWhereAsync(s => !s.OnGoing, false)).OrderByDescending(s => s.Index).First();

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var currentMMRs = (await _mmrs.GetWhereAsync(m => m.SeasonId == currentSeason.Id, false)).Select(x => x.Id);

                    await _mmrs.DeleteRangeAsync(currentMMRs);

                    var currentRecords = (await _records.GetWhereAsync(x => x.Match.SeasonId == currentSeason.Id)).Select(x => x.Id);

                    await _records.DeleteRangeAsync(currentRecords);

                    var currentMatches = (await _matches.GetWhereAsync(x => x.SeasonId == currentSeason.Id)).Select(x => x.Id);

                    await _matches.DeleteRangeAsync(currentMatches);

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
