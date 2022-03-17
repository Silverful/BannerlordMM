using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Domain.Settings;
using BL.API.UnitTests.Builders;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.API.UnitTests.Utility
{
    public static class MatchUtility
    {
        public static Match CreateBaseMatch(
            byte teamWon, 
            byte roundsPlayed, 
            IEnumerable<(byte, int, byte)> recordScores,
            Season season,
            Region region)
        {
            var players = new Faker<Player>()
                .RuleFor(p => p.Id, (f) => Guid.NewGuid())
                .RuleFor(p => p.Nickname, (f) => f.Internet.UserName())
                .RuleFor(p => p.Country, (f) => f.Address.Country())
                .RuleFor(p => p.DiscordId, (f) => f.Random.Int())
                .RuleFor(p => p.Clan, (f) => f.Company.CompanyName())
                .RuleFor(p => p.MainClass, (f) => (PlayerClass)f.Random.Int(1, 3))
                .RuleFor(p => p.SecondaryClass, (f) => (PlayerClass)f.Random.Int(1, 3))
                .Generate(12)
                .ToList();

            players.Select(p => p.PlayerMMRs = new[] { new PlayerMMRBuilder()
                    .WithId(null)
                    .WithMMR(1000)
                    .WithSeason(season)
                    .WithRegion(region)
                    .WithPlayer(p)
                    .Build() });

            var match = new MatchBuilder()
                .WithScreenshotLink("http://whatever.com")
                .WithTeamWon(teamWon)
                .WithId(null)
                .WithRegion(region)
                .WithSeason(season)
                .WithRoundsPlayed(roundsPlayed)
                .Build();

            var matchRecords = recordScores.Select((p, i) => new PlayerRecordBuilder()
                .WithMatch(match)
                .WithPlayer(players[i])
                .WithTeamIndex(p.Item1)
                .WithRoundsWon(p.Item1 == teamWon ? (byte)3 : (byte)(roundsPlayed - 3))
                .WithScore(p.Item2)
                .WithCalibrationIndex(p.Item3)
                .Build())
                .ToList();

            match.PlayerRecords = matchRecords;

            return match;
        }
    }
}
