using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Domain.Settings;
using BL.API.Services.MMR;
using BL.API.Services.Seasons;
using BL.API.UnitTests.Builders;
using Bogus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Match = BL.API.Core.Domain.Match.Match;

namespace BL.API.UnitTests.Services.MMR
{
    public class BetaMMRCalculationAsyncTests
    {
        private readonly IOptions<BasicMMRCalculationProperties> _options;
        private readonly Region _region;
        private readonly Season _season;
        private readonly IMMRCalculationBuilder _builder;
        private readonly ISeasonResolverService _seasonResolver;

        public BetaMMRCalculationAsyncTests()
        {
            _options = Options.Create(new BasicMMRCalculationProperties
            {
                DefaultChange = 20,
                AdditionalBank = 40,
                CalibrationIndexFactor = 4
            });

            _region = new RegionBuilder()
                .WithId(null)
                .WithShortName("eu")
                .WithName("Europe")
                .Build();

            _season = new SeasonBuilder()
                .WithId(null)
                .WithRegion(_region)
                .WithTitle("Beta")
                .WithOnGoing(true)
                .Build();

            var loggerMock = new Mock<ILogger<ISeasonResolverService>>();
            var repMoq = new Mock<IRepository<Season>>();

            var seasonResolverMock = new Mock<ISeasonResolverService>();
            seasonResolverMock
                .Setup(r => r.GetCurrentSeasonAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_season);

            seasonResolverMock
                .Setup(r => r.GetSeasonOnDateAsync(It.IsAny<DateTime>(), It.IsAny<Guid>()))
                .ReturnsAsync(_season);

            _seasonResolver = seasonResolverMock.Object;

            
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
                .Generate(12)
                .ToList();

            players.Select(p => p.PlayerMMRs = new[] { new PlayerMMRBuilder()
                    .WithId(null)
                    .WithMMR(1000)
                    .WithSeason(_season)
                    .WithRegion(_region)
                    .WithPlayer(p)
                    .Build() });

            var match = new MatchBuilder()
                .WithScreenshotLink("http://whatever.com")
                .WithTeamWon(teamWon)
                .WithId(null)
                .WithRegion(_region)
                .WithSeason(_season)
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
        public async Task CorrectMMRCalculation_RealExamplesNoCalibration_ArraysIdentical()
        {
            //Arrange
            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new BetaSeasonStrategy(_options.Value));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);

            var testScores = new List<(byte, int, byte)>
            {
                (1, 1502, 0), (1, 1383, 0), (1, 1357, 0), (1, 1287, 0), (1, 1197, 0), (1, 739, 0),
                (0, 1775, 0), (0, 1653, 0), (0, 1239, 0), (0, 899, 0), (0, 496, 0), (0, 371, 0)
            };

            var realMMRChange = new List<double>
            {
                28, 27, 27, 26, 26, 23,
                -21, -22, -25, -27, -29, -30
            };

            var testMatch = CreateBaseMatch(1, 5, testScores);

            //Act
            var testMMRChange = (await Task.WhenAll(testMatch.PlayerRecords.Select(pr => mmrService.CalculateMMRChangeAsync(pr)))).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }
        
        [Fact]
        public async Task CorrectMMRCalculation_RealExamplesWithCalibration_ArraysIdentical()
        {
            //Arrange
            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new BetaSeasonStrategy(_options.Value));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);

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
            var testMMRChange = (await Task.WhenAll(testMatch.PlayerRecords.Select(pr => mmrService.CalculateMMRChangeAsync(pr)))).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }

        [Fact]
        public async Task CorrectMMRCalculation_ZeroScoreUpload_ArraysReturnDefaultChange()
        {
            //Arrange
            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new BetaSeasonStrategy(_options.Value));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);

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
            var testMMRChange = (await Task.WhenAll(testMatch.PlayerRecords.Select(pr => mmrService.CalculateMMRChangeAsync(pr)))).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }

        [Fact]
        public async Task CorrectMMRCalculation_RealExamplesWithCalibration2_ArraysIdentical()
        {
            //Arrange
            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new BetaSeasonStrategy(_options.Value));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);

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
            var testMMRChange = (await Task.WhenAll(testMatch.PlayerRecords.Select(pr => mmrService.CalculateMMRChangeAsync(pr)))).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }

        [Fact]
        public async Task CorrectMMRCalculation_ZeroAdditionalBank_ArraysIdentical()
        {
            var options = Options.Create(new BasicMMRCalculationProperties
            {
                DefaultChange = 27,
                AdditionalBank = 0,
                CalibrationIndexFactor = 4
            });

            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new BetaSeasonStrategy(options.Value));

            var mmrService = new MMRCalculationService(options, _seasonResolver, builderMock.Object);

            //Arrange
            var testScores = new List<(byte, int, byte)>
            {
                (0, 714, 0), (0, 707, 0), (0, 650, 0), (0, 573, 0), (0, 356, 0), (0, 135, 0),
                (1, 1194, 10), (1, 1069, 0), (1, 801, 0), (1, 634, 0), (1, 614, 0), (1, 574, 0)
            };

            var realMMRChange = new List<int>
            {
                -27, -27, -27, -27, -27, -27,
                108, 27, 27, 27, 27, 27
            };

            var testMatch = CreateBaseMatch(1, 5, testScores);

            //Act
            var testMMRChange = (await Task.WhenAll(testMatch.PlayerRecords.Select(pr => mmrService.CalculateMMRChangeAsync(pr)))).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }
    }
}
