using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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
            public byte? Kills { get; set; }
            public byte? Assists { get; set; }
            public byte? Score { get; set; }
            public byte? MVPs { get; set; }

            public PlayerMatchRecord ToPlayerMatchRecord(byte teamIndex)
            {
                var faction = (Faction)Enum.Parse(typeof(Faction), this.Faction);

                return new PlayerMatchRecord
                {
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
            private readonly IRepository<Match> _repository;

            public UploadMatchCommandHandler(IRepository<Match> repository)
            {
                _repository = repository;
            }

            public async Task<Guid> Handle(UploadMatchCommand request, CancellationToken cancellationToken)
            {
                if (await _repository.GetFirstWhereAsync(m => m.ScreenshotLink == request.ScreenshotLink) != null) throw new AlreadyExistsException();

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
                    record.MMRChange = 0;
                }

                await _repository.CreateAsync(match);
                return match.Id;
            }
        }
    }
}
