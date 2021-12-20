using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Matches.Commands
{
    public class UploadMatchCommand : IRequest<Guid>
    {
        [Required]
        public string ScreenshotLink { get; set; }
        [Required]
        public DateTime MatchDate { get; set; }
        [Required, Range(3, 5)]
        public byte RoundsPlayed { get; set; }
        [Required, MaxLength(6)]
        public List<MatchRecord> Team1Records { get; set; }
        [Required, MaxLength(6)]
        public List<MatchRecord> Team2Records { get; set; }

        public class MatchRecord
        {
            public Guid? PlayerId { get; set; }
            public byte RoundsWon { get; set; }
            public string Faction { get; set; }
            public sbyte? Kills { get; set; }
            public sbyte? Assists { get; set; }
            public int? Score { get; set; }
            [Range(0, 5)]
            public byte? MVPs { get; set; }

            public PlayerMatchRecord ToPlayerMatchRecord(byte teamIndex)
            {
                Faction? faction = null;

                if (this.Faction != null)
                {
                    faction = (Faction)Enum.Parse(typeof(Faction), this.Faction);
                };

                return new PlayerMatchRecord
                {
                    PlayerId = this.PlayerId,
                    TeamIndex = teamIndex,
                    RoundsWon = this.RoundsWon,
                    Faction = faction,
                    Kills = this.Kills,
                    Assists = this.Assists,
                    Score = this.Score,
                    MVPs = this.MVPs
                };
            }
        }

        public class UploadMatchCommandHandler: IRequestHandler<UploadMatchCommand, Guid>
        {
            private readonly IRepository<Match> _matchRepository;
            private readonly IRepository<PlayerMatchRecord> _playerRecords;
            private readonly IRepository<Player> _players;
            private readonly IMMRCalculationService _mmrCalculation;
            private readonly ILogger<UploadMatchCommandHandler> _logger;

            public UploadMatchCommandHandler(IRepository<Match> matchRepository, 
                IRepository<PlayerMatchRecord> playerRecords,
                IRepository<Player> players,
                IMMRCalculationService mmrCalculation,
                ILogger<UploadMatchCommandHandler> logger)
            {
                _matchRepository = matchRepository;
                _playerRecords = playerRecords;
                _players = players;
                _mmrCalculation = mmrCalculation;
                _logger = logger;
            }

            public async Task<Guid> Handle(UploadMatchCommand request, CancellationToken cancellationToken)
            {
                if ((await _matchRepository.GetFirstWhereAsync(m => m.ScreenshotLink == request.ScreenshotLink)) != null) throw new AlreadyExistsException();

                var match = new Match()
                {
                    ScreenshotLink = request.ScreenshotLink,
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

                await _matchRepository.CreateAsync(match);

                //UNSAFE CHANGE TO TRANSACTION
                foreach (var record in match.PlayerRecords)
                {
                    if (record.PlayerId.HasValue && record.MMRChange.HasValue)
                    {
                        var player = await _players.GetByIdAsync(record.PlayerId.Value);

                        player.PlayerMMR += record.MMRChange.Value;
                        await _players.UpdateAsync(player);
                    }
                }

                _logger?.LogInformation($"Match created {JsonSerializer.Serialize(match, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve })}");

                return match.Id;
            }
        }
    }
}
