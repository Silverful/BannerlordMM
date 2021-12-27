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
            private readonly ISeasonResolverService _seasonService;
            private readonly IMediator _mediator;

            public UpdateMatchCommandHandler(IRepository<Match> matchRepository,
                IRepository<PlayerMatchRecord> playerRecords,
                IRepository<Player> players,
                IMMRCalculationService mmrCalculation,
                ISeasonResolverService seasonService,
                IMediator mediator,
                ILogger<UpdateMatchCommandHandler> logger)
            {
                _matchRepository = matchRepository;
                _playerRecords = playerRecords;
                _players = players;
                _mmrCalculation = mmrCalculation;
                _seasonService = seasonService;
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<Task> Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
            {
                 var match = await _matchRepository.GetByIdAsync(request.MatchId, false);

                if (match == null) throw new NotFoundException();

                var matchSeason = await _seasonService.GetSeasonOnDateAsync(match.MatchDate);

                match.ScreenshotLink = request.ScreenshotLink;
                match.MatchDate = request.MatchDate;
                match.RoundsPlayed = request.RoundsPlayed;
                match.TeamWon = (byte)(request.Team1Records.First().RoundsWon > request.Team2Records.First().RoundsWon ? 1 : 2);

                //reduce old MMR changes
                var oldRecords = match.PlayerRecords.ToList();
                oldRecords.ForEach(pr => { pr.Player.PlayerMMR.MMR -= pr.MMRChange ?? 0; });
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

                

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _playerRecords.DeleteRangeAsync(oldRecords);
                    await _players.UpdateRangeAsync(reversedPlayers);
                    await _matchRepository.UpdateAsync(match);

                    //var playersToUpdate = new List<Player>();

                    //foreach (var record in match.PlayerRecords)
                    //{
                    //    if (record.PlayerId.HasValue && record.MMRChange.HasValue)
                    //    {
                    //        var player = reversedPlayers.Where(p => p.Id == record.PlayerId).FirstOrDefault() ?? await _players.GetByIdAsync(record.PlayerId.Value);

                    //        player.PlayerMMR.MMR += record.MMRChange.Value;
                    //        playersToUpdate.Add(player);
                    //    }
                    //}
                    //await _players.UpdateRangeAsync(playersToUpdate);

                    scope.Complete();
                }

                foreach (var rec in match.PlayerRecords)
                {
                    await _mediator.Send(new ReloadPlayersRecordsCommand.Query(rec));
                }

                _logger?.LogInformation($"Match updated {JsonSerializer.Serialize(match, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve })}");

                return Task.CompletedTask;
            }
        }
    }
}
