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
    public class UploadMatchCommand : UploadMatchRequest, IRequest<Guid> 
    {
        public class UploadMatchCommandHandler: IRequestHandler<UploadMatchCommand, Guid>
        {
            private readonly IRepository<Match> _matchRepository;
            private readonly IRepository<PlayerMatchRecord> _playerRecords;
            private readonly IRepository<Player> _players;
            private readonly IMMRCalculationService _mmrCalculation;
            private readonly ILogger<UploadMatchCommandHandler> _logger;
            private readonly ISeasonResolverService _seasonService;

            public UploadMatchCommandHandler(IRepository<Match> matchRepository, 
                IRepository<PlayerMatchRecord> playerRecords,
                IRepository<Player> players,
                IMMRCalculationService mmrCalculation,
                ISeasonResolverService seasonService,
                ILogger<UploadMatchCommandHandler> logger)
            {
                _matchRepository = matchRepository;
                _playerRecords = playerRecords;
                _players = players;
                _mmrCalculation = mmrCalculation;
                _seasonService = seasonService;
                _logger = logger;
            }

            public async Task<Guid> Handle(UploadMatchCommand request, CancellationToken cancellationToken)
            {
                if ((await _matchRepository.GetFirstWhereAsync(m => m.ScreenshotLink == request.ScreenshotLink)) != null) throw new AlreadyExistsException();

                var season = await _seasonService.GetCurrentSeasonAsync();

                var match = new Match()
                {
                    ScreenshotLink = request.ScreenshotLink,
                    Season = season,
                    SeasonId = season.Id,
                    MatchDate = request.MatchDate,
                    RoundsPlayed = request.RoundsPlayed,
                    TeamWon = (byte)(request.Team1Records.First().RoundsWon > request.Team2Records.First().RoundsWon ? 1 : 2)
                };

                match.PlayerRecords = request.Team1Records.Select(t1 => t1.ToPlayerMatchRecord(1))
                    .Concat(request.Team2Records.Select(t2 => t2.ToPlayerMatchRecord(2)))
                    .ToList();


                foreach (var record in match.PlayerRecords)
                {
                    record.Match = match;

                    if (record.PlayerId.HasValue)
                    {
                        var playerMatchRecordCount = (await _playerRecords.GetWhereAsync(pr => pr.PlayerId == record.PlayerId)).Count();

                        record.CalibrationIndex = (byte)(playerMatchRecordCount >= 10 ? 0 : 10 - playerMatchRecordCount);
                        record.MMRChange = _mmrCalculation.CalculateMMRChange(record);
                    }
                }

                using (var scope = new TransactionScope())
                {
                    await _matchRepository.CreateAsync(match);

                    var playersToUpdate = new List<Player>();

                    foreach (var record in match.PlayerRecords)
                    {
                        if (record.PlayerId.HasValue && record.MMRChange.HasValue)
                        {
                            var player = await _players.GetByIdAsync(record.PlayerId.Value);

                            player.PlayerMMR += record.MMRChange.Value;
                            playersToUpdate.Add(player);
                        }
                    }

                    await _players.UpdateRangeAsync(playersToUpdate);
                }
                

                _logger?.LogInformation($"Match created {JsonSerializer.Serialize(match, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve })}");

                return match.Id;
            }
        }
    }
}
