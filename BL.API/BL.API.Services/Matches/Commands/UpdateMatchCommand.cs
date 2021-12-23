using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BL.API.Services.Matches.Commands
{
    public class UpdateMatchCommand : UpdateMatchRequest, IRequest<Task>
    {
        public class UpdateMatchCommandHandler : IRequestHandler<UpdateMatchCommand, Task>
        {
            private readonly IRepository<Match> _matchRepository;
            private readonly IRepository<PlayerMatchRecord> _playerRecords;
            private readonly IRepository<Player> _players;
            private readonly IMMRCalculationService _mmrCalculation;
            private readonly ILogger<UpdateMatchCommandHandler> _logger;

            public UpdateMatchCommandHandler(IRepository<Match> matchRepository,
                IRepository<PlayerMatchRecord> playerRecords,
                IRepository<Player> players,
                IMMRCalculationService mmrCalculation,
                ILogger<UpdateMatchCommandHandler> logger)
            {
                _matchRepository = matchRepository;
                _playerRecords = playerRecords;
                _players = players;
                _mmrCalculation = mmrCalculation;
                _logger = logger;
            }

            public async Task<Task> Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
            {
                var match = await _matchRepository.GetByIdAsync(request.MatchId);

                if (match == null) throw new NotFoundException();

                match.ScreenshotLink = request.ScreenshotLink;
                match.MatchDate = request.MatchDate;
                match.RoundsPlayed = request.RoundsPlayed;
                match.TeamWon = (byte)(request.Team1Records.First().RoundsWon > request.Team2Records.First().RoundsWon ? 1 : 2);

                //reduce old MMR changes
                var oldRecords = match.PlayerRecords.ToList();
                oldRecords.ForEach(pr => { pr.Player.PlayerMMR -= pr.MMRChange ?? 0; });
                var reversedPlayers = match.PlayerRecords.Select(pr => pr.Player).ToList();

                var updatedRecords = request.Team1Records.Select(t1 => t1.ToPlayerMatchRecord(1))
                    .Concat(request.Team2Records.Select(t2 => t2.ToPlayerMatchRecord(2)))
                    .ToList();

                updatedRecords.ForEach(r =>
                {
                    var sameRecord = oldRecords.Where(or => or.PlayerId == r.PlayerId && or.TeamIndex == r.TeamIndex).FirstOrDefault();

                    r.Match = match;
                    r.MatchId = match.Id;
                    r.Id = sameRecord?.Id ?? r.Id;

                    if (sameRecord != null)
                    {
                        r.Created = sameRecord.Created;
                    }

                    if (sameRecord != null)
                    {
                        oldRecords.Remove(sameRecord);
                    }
                });

                match.PlayerRecords = updatedRecords;

                foreach (var record in match.PlayerRecords)
                {
                    if (record.PlayerId.HasValue)
                    {
                        var playerMatchRecordCount = (await _playerRecords.GetWhereAsync(pr => pr.PlayerId == record.PlayerId)).Count();

                        var isExistsAdd = record.Id == Guid.Empty? 0 : 1;

                        record.CalibrationIndex = (byte)((playerMatchRecordCount - isExistsAdd) >= 10 ? 0 : 10 - playerMatchRecordCount + isExistsAdd); //old records still in the db
                        record.MMRChange = _mmrCalculation.CalculateMMRChange(record);
                    }
                }

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _playerRecords.DeleteRangeAsync(oldRecords);
                    await _players.UpdateRangeAsync(reversedPlayers);
                    await _matchRepository.UpdateAsync(match);

                    var playersToUpdate = new List<Player>();

                    foreach (var record in match.PlayerRecords)
                    {
                        if (record.PlayerId.HasValue && record.MMRChange.HasValue)
                        {
                            var player = reversedPlayers.Where(p => p.Id == record.PlayerId).FirstOrDefault() ?? await _players.GetByIdAsync(record.PlayerId.Value);

                            player.PlayerMMR += record.MMRChange.Value;
                            playersToUpdate.Add(player);
                        }
                    }
                    await _players.UpdateRangeAsync(playersToUpdate);

                    scope.Complete();
                }

                _logger?.LogInformation($"Match updated {JsonSerializer.Serialize(match, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve })}");

                return Task.CompletedTask;
            }
        }
    }
}
