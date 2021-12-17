using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        [Required]
        public byte RoundsPlayed { get; set; }
        [Required]
        public IEnumerable<MatchRecord> Team1Records { get; set; }
        [Required]
        public IEnumerable<MatchRecord> Team2Records { get; set; }

        public class MatchRecord
        {
            [Required]
            public Guid PlayerId { get; set; }
            public byte RoundsWon { get; set; }
            public string Faction { get; set; }
            public sbyte? Kills { get; set; }
            public sbyte? Assists { get; set; }
            public int? Score { get; set; }
            public byte? MVPs { get; set; }

            public PlayerMatchRecord ToPlayerMatchRecord(byte teamIndex)
            {
                var faction = (Faction)Enum.Parse(typeof(Faction), this.Faction);

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

            public UploadMatchCommandHandler(IRepository<Match> matchRepository, 
                IRepository<PlayerMatchRecord> playerRecords,
                IRepository<Player> players,
                IMMRCalculationService mmrCalculation)
            {
                _matchRepository = matchRepository;
                _playerRecords = playerRecords;
                _players = players;
                _mmrCalculation = mmrCalculation;
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

                var matchRecords = request.Team1Records.Select(t1 => t1.ToPlayerMatchRecord(1))
                    .Concat(request.Team2Records.Select(t2 => t2.ToPlayerMatchRecord(2)));

                match.PlayerRecords = matchRecords.ToList();

                foreach (var record in match.PlayerRecords)
                {
                    record.Match = match;
                    var playerMatchRecordCount = (await _playerRecords.GetWhereAsync(pr => pr.PlayerId == record.PlayerId)).Count();

                    record.CalibrationIndex = (byte)(playerMatchRecordCount >= 10 ? 0 : 10 - playerMatchRecordCount);
                    record.MMRChange = _mmrCalculation.CalculateMMRChange(record);
                }

                await _matchRepository.CreateAsync(match);

                //UNSAFE CHANGE TO TRANSACTION
                foreach (var record in match.PlayerRecords)
                {
                    var player = await _players.GetByIdAsync(record.PlayerId);

                    player.PlayerMMR += record.MMRChange;
                    await _players.UpdateAsync(player);
                }

                return match.Id;
            }
        }
    }
}
