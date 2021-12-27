using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Services.MMR;
using BL.API.UnitTests.Builders;
using Bogus;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BL.API.UnitTests.Services.MMR
{
    public class CorrectMMRCalculationAsyncTests
    {
        private readonly MMRCalculationService _mmrService;

        public CorrectMMRCalculationAsyncTests()
        {
            var options = Options.Create(new BasicMMRCalculationProperties
            {
                DefaultChange = 20,
                AdditionalBank = 40
            });

            _mmrService = new MMRCalculationService(options);
        }

        public Match CreateBaseMatch(byte teamWon, byte roundsPlayed, IEnumerable<(byte, int, byte)> recordScores)
        {
            var players = new Faker<Player>()
                .RuleFor(p => p.Id, (f) => Guid.NewGuid())
                .RuleFor(p => p.Nickname, (f) => f.Internet.UserName())
                .RuleFor(p => p.Country, (f) => f.Address.Country())
                .RuleFor(p => p.DiscordId, (f) => f.Random.Int())
                .RuleFor(p => p.Clan, (f) => f.Company.CompanyName())
                .RuleFor(p => p.MainClass, (f) => (PlayerClass)f.Random.Int(1, 3))
                .RuleFor(p => p.SecondaryClass, (f) => (PlayerClass)f.Random.Int(1, 3))
                .RuleFor(p => p.PlayerMMR, (f) => f.Random.Int(0, 1000))
                .Generate(12)
                .ToList();

            var match = new MatchBuilder()
                .WithScreenshotLink("http://whatever.com")
                .WithTeamWon(teamWon)
                .WithId(null)
                .WithRoundsPlayed(roundsPlayed)
                .Build();

            var matchRecords = recordScores.Select((p, i) => new PlayerRecordBuilder()
                .WithMatch(match)
                .WithPlayer(players[i])
                .WithTeamIndex(p.Item1)
                .WithScore(p.Item2) 
                .WithCalibrationIndex(p.Item3)
                .Build())
                .ToList();

            match.PlayerRecords = matchRecords;

            return match;
        }

        [Fact]
        public void CorrectMMRCalculation_RealExamplesNoCalibration_ArraysIdentical()
        {
            //Arrange
            var testScores = new List<(byte, int, byte)>
            {
                (1, 1502, 0), (1, 1383, 0), (1, 1357, 0), (1, 1287, 0), (1, 1197, 0), (1, 739, 0),
                (0, 1775, 0), (0, 1653, 0), (0, 1239, 0), (0, 899, 0), (0, 496, 0), (0, 371, 0)
            };

            var realMMRChange = new List<int>
            {
                28, 27, 27, 26, 26, 23,
                -21, -22, -25, -27, -29, -30
            };

            var testMatch = CreateBaseMatch(1, 5, testScores);

            //Act
            var testMMRChange = testMatch.PlayerRecords.Select(pr => _mmrService.CalculateMMRChange(pr)).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }
        
        [Fact]
        public void CorrectMMRCalculation_RealExamplesWithCalibration_ArraysIdentical()
        {
            //Arrange
            var testScores = new List<(byte, int, byte)>
            {
                (1, 1715, 0), (1, 1488, 0), (1, 1227, 0), (1, 980, 5), (1, 895, 1), (1, 680, 0),
                (0, 1964, 0), (0, 1205, 0), (0, 1193, 0), (0, 941, 0), (0, 726, 0), (0, 554, 7)
            };

            var realMMRChange = new List<int>
            {
                29, 28, 27, 100, 100, 23,
                -21, -25, -25, -27, -28, 0
            };

            var testMatch = CreateBaseMatch(1, 5, testScores);

            //Act
            var testMMRChange = testMatch.PlayerRecords.Select(pr => _mmrService.CalculateMMRChange(pr)).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }

        [Fact]
        public void CorrectMMRCalculation_ZeroScoreUpload_ArraysReturnDefaultChange()
        {
            //Arrange
            var testScores = new List<(byte, int, byte)>
            {
                (1, 0, 0), (1, 0, 0), (1, 0, 0), (1, 0, 0), (1, 0, 0), (1, 0, 0),
                (0, 0, 0), (0, 0, 0), (0, 0, 0), (0, 0, 0), (0, 0, 0), (0, 0, 0)
            };

            var realMMRChange = new List<int>
            {
                20, 20, 20, 20, 20, 20,
                -20, -20, -20, -20, -20, -20
            };

            var testMatch = CreateBaseMatch(1, 5, testScores);

            //Act
            var testMMRChange = testMatch.PlayerRecords.Select(pr => _mmrService.CalculateMMRChange(pr)).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }

        [Fact]
        public void CorrectMMRCalculation_RealExamplesWithCalibration2_ArraysIdentical()
        {
            //Arrange
            var testScores = new List<(byte, int, byte)>
            {
                (0, 714, 0), (0, 707, 0), (0, 650, 3), (0, 573, 0), (0, 356, 0), (0, 135, 10),
                (1, 1194, 0), (1, 1069, 10), (1, 801, 0), (1, 634, 5), (1, 614, 6), (1, 574, 8)
            };

            var realMMRChange = new List<int>
            {
                -23, -23, 0, -25, -28, 0,
                29, 112, 26, 100, 100, 96
            };

            var testMatch = CreateBaseMatch(1, 5, testScores);

            //Act
            var testMMRChange = testMatch.PlayerRecords.Select(pr => _mmrService.CalculateMMRChange(pr)).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }

        [Fact]
        public void CorrectMMRCalculation_ZeroAdditionalBank_ArraysIdentical()
        {
            var options = Options.Create(new BasicMMRCalculationProperties
            {
                DefaultChange = 27,
                AdditionalBank = 0
            });

            var mmrService = new MMRCalculationService(options);

            //Arrange
            var testScores = new List<(byte, int, byte)>
            {
                (0, 714, 0), (0, 707, 0), (0, 650, 0), (0, 573, 0), (0, 356, 0), (0, 135, 0),
                (1, 1194, 0), (1, 1069, 0), (1, 801, 0), (1, 634, 0), (1, 614, 0), (1, 574, 0)
            };

            var realMMRChange = new List<int>
            {
                -27, -27, -27, -27, -27, -27,
                27, 27, 27, 27, 27, 27
            };

            var testMatch = CreateBaseMatch(1, 5, testScores);

            //Act
            var testMMRChange = testMatch.PlayerRecords.Select(pr => mmrService.CalculateMMRChange(pr)).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }
    }
}
