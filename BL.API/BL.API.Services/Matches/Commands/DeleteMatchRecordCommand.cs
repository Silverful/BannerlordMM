using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Matches.Commands
{
    public class DeleteMatchRecordCommand
    {
        public record Query(Guid RecordId) : IRequest<Task>;

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
                var record = await _matchRecords.GetByIdAsync(request.RecordId, false, r => r.Match, r => r.Player);
                var match = record.Match;
                var player = record.Player;
                var regionId = match.RegionId.Value;

                if (record.PlayerId.HasValue
                    && record.MMRChange.HasValue
                    && record.CalibrationIndex.HasValue)
                {
                    var playerMMR = player.GetPlayerMMR(match.RegionId.Value);
                    playerMMR.MMR = playerMMR.MMR - record.MMRChange.Value;

                    if (record.CalibrationIndex > 0)
                    {
                        var calibrationRecords = (await _matchRecords
                        .GetWhereAsync(pr =>
                            pr.MatchId != match.Id
                            && pr.Match.RegionId == regionId
                            && pr.Match.SeasonId == match.SeasonId
                            && pr.PlayerId == record.PlayerId, false, pr => pr.Match, pr => pr.Player))
                        .OrderByDescending(pr => pr.CalibrationIndex)
                        .ThenByDescending(pr => pr.Match.MatchDate)
                        .ThenByDescending(pr => pr.Match.Created)
                        .Take(11)
                        .ToList();

                        byte calibrationIndex = 10;
                        foreach (var rec in calibrationRecords)
                        {
                            rec.CalibrationIndex = calibrationIndex;
                            var newMMRChange = await _mmrService.CalculateMMRChangeAsync(rec);
                            playerMMR.MMR = playerMMR.MMR - rec.MMRChange.Value + newMMRChange;
                            rec.MMRChange = newMMRChange;
                            calibrationIndex--;
                        }

                        await _matchRecords.UpdateRangeAsync(calibrationRecords);
                    }

                    await _players.UpdateAsync(player);
                }

                await _matchRecords.DeleteAsync(record);

                return Task.CompletedTask;
            }
        }
    }
}
