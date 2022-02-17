using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Services.Players.Commands;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BL.API.Services.Matches.Commands
{
    public class LoadPlayersRecordCommand
    {
        public record Query(PlayerMatchRecord record) : IRequest<Task>;
        /// <summary>
        /// Reloads playerRecord to update the MMR and also (!) reload next games to ensure calibration is not broken
        /// </summary>
        public class LoadPlayersRecordCommandHandler : IRequestHandler<Query, Task>
        {
            private readonly IRepository<PlayerMatchRecord> _playerRecords;
            private readonly IRepository<Player> _players;
            private readonly IMediator _mediator;
            private readonly IMMRCalculationService _mmrCalculation;

            public LoadPlayersRecordCommandHandler(IRepository<PlayerMatchRecord> playerRecords,
                    IRepository<Player> players,
                    IMediator mediator,
                    IMMRCalculationService mmrCalculation)
            {
                _playerRecords = playerRecords;
                _players = players;
                _mediator = mediator;
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
                        && pr.Match.MatchDate <= redoMatch.MatchDate, true, pr => pr.Player, pr => pr.Match))
                    .OrderBy(pr => pr.CalibrationIndex)
                    .Take(1)
                    .FirstOrDefault();

                //get current record in the right place
                byte calibrationIndex = (byte)(precedingRecord == null ? 10 : (!precedingRecord.CalibrationIndex.HasValue || precedingRecord.CalibrationIndex == 0? 0 : precedingRecord.CalibrationIndex - 1));

                redoRecord.CalibrationIndex = calibrationIndex;
                var mmrChange = await _mmrCalculation.CalculateMMRChangeAsync(redoRecord);

                var updatedPlayer = await _players.GetFirstWhereAsync(p => p.Id == redoRecord.PlayerId, false);

                if (updatedPlayer != null && updatedPlayer.PlayerMMR == null)
                {
                    var mmr = await _mediator.Send(new CreateNewPlayerMMRCommand() { PlayerId = updatedPlayer.Id, SeasonId = redoMatch.SeasonId.Value });
                    updatedPlayer.PlayerMMRs.Add(mmr);
                }

                updatedPlayer.PlayerMMR.MMR = updatedPlayer.PlayerMMR.MMR - (redoRecord.MMRChange ?? 0) + mmrChange;
                redoRecord.MMRChange = mmrChange;

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _playerRecords.UpdateAsync(redoRecord);

                    if (calibrationIndex > 0)
                    {
                        var calibrationRecords = (await _playerRecords
                        .GetWhereAsync(pr =>
                            //pr.MatchId != redoMatch.Id &&
                            pr.Match.SeasonId == redoMatch.SeasonId &&
                            pr.PlayerId == redoRecord.PlayerId &&
                            pr.CalibrationIndex.HasValue &&
                            pr.CalibrationIndex.Value > 0, false, pr => pr.Match, pr => pr.Player))
                        .OrderByDescending(pr => pr.CalibrationIndex)
                        .ThenByDescending(pr => pr.Match.MatchDate)
                        .ThenByDescending(pr => pr.Match.Created)
                        .ToList();

                        calibrationIndex = 10;

                        if (calibrationRecords.Count() > 0)
                        {
                            foreach (var sucRec in calibrationRecords)
                            {
                                sucRec.CalibrationIndex = calibrationIndex;
                                if (sucRec.CalibrationIndex < 0)
                                {
                                    sucRec.CalibrationIndex = 0;
                                }

                                var sMMRChange = await _mmrCalculation.CalculateMMRChangeAsync(sucRec);
                                updatedPlayer.PlayerMMR.MMR = updatedPlayer.PlayerMMR.MMR - (sucRec.MMRChange ?? 0) + sMMRChange;
                                sucRec.MMRChange = sMMRChange;
                                calibrationIndex--;
                            }

                            await _playerRecords.UpdateRangeAsync(calibrationRecords);
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