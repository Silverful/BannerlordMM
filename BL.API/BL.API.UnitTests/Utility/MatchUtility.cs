using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Domain.Settings;
using BL.API.Services.Matches.Commands;
using BL.API.UnitTests.Builders;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using static BL.API.Services.Matches.Commands.UploadMatchRequest;

namespace BL.API.UnitTests.Utility
{
    public static class MatchUtility
    {
        public static IEnumerable<Player> CreatePlayers(int number,
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
                .Generate(number)
                .ToList();

            players.Select(p => p.PlayerMMRs = new[] { new PlayerMMRBuilder()
                    .WithId(null)
                    .WithMMR(1000)
                    .WithSeason(season)
                    .WithRegion(region)
                    .WithPlayer(p)
                    .Build() });

            return players;
        }

        public static UploadMatchCommand CreateMatchCommand(Region region, IList<Player> players = null)
        {
            var rnd = new Random();

            byte roundsWon = (byte)rnd.Next(3, 5);
            byte roundsLost = (byte)rnd.Next(0, roundsWon - 1);
            var faction1 = (Faction)rnd.Next(1, 6);
            var faction2 = (Faction)rnd.Next(1, 6);

            var faker = new Faker<MatchRecord>()
                .RuleFor(p => p.Kills, f => f.Random.SByte(-2, 20))
                .RuleFor(p => p.Deaths, f => f.Random.Byte(0, 20))
                .RuleFor(p => p.Assists, f => f.Random.SByte(-2, 20))
                .RuleFor(p => p.Score, f => f.Random.Int(-100, 2000))
                .RuleFor(p => p.MVPs, f => f.Random.Byte(0, 5));

            var team1 = faker.Generate(6);

            int i = 0;
            team1.ForEach(r =>
            {
                if (players != null)
                {
                    r.PlayerId = players[i].Id;
                }
                r.RoundsWon = roundsWon;
                r.Faction = faction1.ToString();
                i++;
            });

            var team2 = faker.Generate(6);

            team2.ForEach(r =>
            {
                if (players != null)
                {
                    r.PlayerId = players[i].Id;
                }
                r.Faction = faction2.ToString();
                r.RoundsWon = roundsLost;
                i++;
            });

            return new Faker<UploadMatchCommand>()
                .RuleFor(p => p.ScreenshotLink, f => f.Internet.Url())
                .RuleFor(p => p.MatchDate, f => f.Date.Recent())
                .RuleFor(p => p.RoundsPlayed, f => (byte)5)
                .RuleFor(p => p.Team1Records, f => team1)
                .RuleFor(p => p.Team2Records, f => team2)
                .RuleFor(p => p.RegionShortName, f => region.ShortName)
                .Generate();
        }


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
