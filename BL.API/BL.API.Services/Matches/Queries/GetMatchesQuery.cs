using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Matches.Queries
{
    public static class GetMatchesQuery 
    {
        public record Query() : IRequest<IEnumerable<PlayerMatchResponse>>;

        public class GetMatchesQueryHandler : IRequestHandler<Query, IEnumerable<PlayerMatchResponse>>
        {
            private readonly IRepository<Match> _repository;

            public GetMatchesQueryHandler(IRepository<Match> repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<PlayerMatchResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var matchRecords = (await _repository.GetAllAsync())
                    .OrderBy(m => m.MatchDate)
                    .SelectMany(m => m.PlayerRecords)
                    .Select(mr => new PlayerMatchResponse
                    {
                        Date = mr.Match.MatchDate,
                        Nickname = mr.Player.Nickname,
                        Played = (byte)1,
                        Wins = (byte)(mr.TeamIndex == mr.Match.TeamWon ? 1 : 0),
                        Rounds = mr.Match.RoundsPlayed,
                        Kills = mr.Kills,
                        Assists = mr.Assists,
                        Score = mr.Score,
                        ScreenshotLink = mr.Match.ScreenshotLink,
                        CalibrationIndex = 0
                    })
                    .ToList();

                return matchRecords;
            }
        }

        public class PlayerMatchResponse
        {
            public DateTime Date { get; set; }
            public string Nickname { get; set; }
            public byte? Played { get; set; }
            public byte? Wins { get; set; }
            public byte? Rounds { get; set; }
            public byte? Kills { get; set; }
            public byte? Assists { get; set; }
            public int? Score { get; set; }
            public string ScreenshotLink { get; set; }
            public byte CalibrationIndex { get; set; }
        }
    }
}
