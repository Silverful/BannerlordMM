using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using static BL.API.Services.Matches.Commands.UpdateMatchCommand;

namespace BL.API.Services.Matches.Commands
{
    public class ReloadPlayersRecordsCommand
    {
        public record Query(PlayerMatchRecord record) : IRequest<Task>;
        /// <summary>
        /// Reloads playerRecord to update the MMR and also (!) reload next games to ensure calibration is not broken
        /// </summary>
        public class ReloadPlayersRecordsCommandHandler : IRequestHandler<Query, Task>
        {
            private readonly IRepository<PlayerMatchRecord> _playerRecords;
            private readonly IRepository<Player> _players;
            private readonly IMMRCalculationService _mmrCalculation;

            public ReloadPlayersRecordsCommandHandler(IRepository<PlayerMatchRecord> playerRecords,
                    IRepository<Player> players,
                    IMMRCalculationService mmrCalculation)
            {
                _playerRecords = playerRecords;
                _players = players;
                _mmrCalculation = mmrCalculation;
            }

            public async Task<Task> Handle (Query request, CancellationToken cancellationToken)
            {
                var redoRecord = request.record;
                var redoMatch = request.record.Match;

                if (!redoRecord.PlayerId.HasValue || redoRecord.Match == null)
                {
                    return Task.CompletedTask;
                }

                var precedingRecord = (await _playerRecords
                    .GetWhereAsync(pr =>
                        pr.MatchId != redoMatch.Id
                        && pr.Match.SeasonId == redoMatch.SeasonId
                        && pr.PlayerId == redoRecord.PlayerId
                        && pr.Match.MatchDate <= redoMatch.MatchDate, true))
                    .OrderBy(pr => pr.CalibrationIndex)
                    .Take(1)
                    .FirstOrDefault();

                //get current record in the right place
                byte calibrationIndex = (byte)(precedingRecord == null ? 10 : (precedingRecord.CalibrationIndex == 0 ? 0 : precedingRecord.CalibrationIndex - 1));

                redoRecord.CalibrationIndex = calibrationIndex;
                var mmrChange = _mmrCalculation.CalculateMMRChange(redoRecord);

                var updatedPlayer = await _players.GetFirstWhereAsync(p => p.Id == redoRecord.PlayerId, false);
                updatedPlayer.PlayerMMR.MMR = updatedPlayer.PlayerMMR.MMR - (redoRecord.MMRChange ?? 0) + mmrChange;
                redoRecord.MMRChange = mmrChange;

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _playerRecords.UpdateAsync(redoRecord);

                    if (calibrationIndex > 0)
                    {
                        var successiveRecords = (await _playerRecords
                        .GetWhereAsync(pr =>
                            pr.MatchId != redoMatch.Id
                            && pr.Match.SeasonId == redoMatch.SeasonId
                            && pr.PlayerId == redoRecord.PlayerId
                            && pr.CalibrationIndex.HasValue
                            && pr.CalibrationIndex.Value <= calibrationIndex, false, pr => pr.Match))
                        .OrderByDescending(pr => pr.CalibrationIndex)
                        .Take(calibrationIndex);

                        if (successiveRecords.Count() > 0)
                        {
                            foreach (var sucRec in successiveRecords)
                            {
                                sucRec.CalibrationIndex = (byte?)(calibrationIndex - 1);
                                if (sucRec.CalibrationIndex < 0)
                                {
                                    sucRec.CalibrationIndex = 0;
                                }

                                var sMMRChange = _mmrCalculation.CalculateMMRChange(sucRec);
                                updatedPlayer.PlayerMMR.MMR = updatedPlayer.PlayerMMR.MMR - (sucRec.MMRChange ?? 0) + sMMRChange;
                                sucRec.MMRChange = sMMRChange;
                                calibrationIndex--;
                            }

                            await _playerRecords.UpdateRangeAsync(successiveRecords);
                        }
                    }

                    await _players.UpdateAsync(updatedPlayer);
                    scope.Complete();
                    return Task.CompletedTask;
                }
            }

            public class ReloadResponse
            {
                public Player Player { get; set; }
                public IEnumerable<PlayerMatchRecord> Records { get; set; }
            }
        }
    }
}