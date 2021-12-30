using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Matches.Commands
{
    public class DeleteMatchRecordCommand
    {
        public record Query(PlayerMatchRecord Record) : IRequest<Task>;

        public class DeleteMatchRecordCommandHandler : IRequestHandler<Query, Task>
        {
            private readonly IRepository<PlayerMatchRecord> _matchRecords;
            private readonly IRepository<Player> _players;
            private readonly IMMRCalculationService _mmrService;

            public DeleteMatchRecordCommandHandler(IRepository<PlayerMatchRecord> matchRecords, 
                IRepository<Player> players,
                IMMRCalculationService mmrService)
            {
                _matchRecords = matchRecords;
                _mmrService = mmrService;
                _players = players;
            }

            public async Task<Task> Handle(Query request, CancellationToken cancellationToken)
            {
                var record = request.Record;
                var match = request.Record.Match;
                var player = request.Record.Player;

                if (record.PlayerId.HasValue
                    && record.MMRChange.HasValue
                    && record.CalibrationIndex.HasValue)
                {
                    player.PlayerMMR.MMR = player.PlayerMMR.MMR - record.MMRChange.Value;

                    if (record.CalibrationIndex > 0)
                    {
                        var succeedingRecords = (await _matchRecords.GetWhereAsync(mr =>
                            mr.Id != record.Id
                            && mr.PlayerId == record.PlayerId
                            && mr.CalibrationIndex < record.CalibrationIndex, false, mr => mr.Match))
                        .OrderByDescending(mr => mr.CalibrationIndex)
                        .Take(record.CalibrationIndex.Value)
                        .ToList();

                        foreach (var rec in succeedingRecords)
                        {
                            rec.CalibrationIndex++;
                            var newMMRChange = _mmrService.CalculateMMRChange(record);
                            player.PlayerMMR.MMR = player.PlayerMMR.MMR - rec.MMRChange.Value + newMMRChange;
                            rec.MMRChange = newMMRChange;
                        }

                        await _matchRecords.UpdateRangeAsync(succeedingRecords);
                    }
                    await _players.UpdateAsync(player);
                }

                await _matchRecords.DeleteAsync(record);

                return Task.CompletedTask;
            }
        }
    }
}
