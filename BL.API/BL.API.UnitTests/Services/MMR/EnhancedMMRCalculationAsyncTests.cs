using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Settings;
using BL.API.Services.MMR;
using BL.API.Services.Players.Queries;
using BL.API.UnitTests.Builders;
using BL.API.UnitTests.Utility;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BL.API.UnitTests.Services.MMR
{
    public class EnhancedMMRCalculationAsyncTests
    {
        private readonly IOptions<BasicMMRCalculationProperties> _options;
        private readonly Region _region;
        private readonly Season _season;
        private readonly ISeasonResolverService _seasonResolver;

        public EnhancedMMRCalculationAsyncTests()
        {
            _options = Options.Create(new BasicMMRCalculationProperties
            {
                DefaultChange = 25,
                AdditionalBank = 0,
                CalibrationIndexFactor = 4,
                AvgCavScore = 213,
                CavPositiveExp = 1.5,
                CavNegativExp = 1.5,
                AvgInfScore = 170,
                InfPositiveExp = 1.4,
                InfNegativeExp = 1.6,
                AvgArcherScore = 208,
                ArchPositiveExp = 1.5,
                ArcherNegativeExp = 1.5,
                Factor = 0.1
            });

            _region = new RegionBuilder()
                .WithId(null)
                .WithShortName("eu")
                .WithName("Europe")
                .Build();

            _season = new SeasonBuilder()
                .WithId(null)
                .WithRegion(_region)
                .WithTitle("Test")
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

        [Fact]
        public async Task CorrectMMRCalculation_NoCalibration30_MMRChangeIsTimes3()
        {
            //Arrange
            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new EnhancedCalibrationStrategy(_options.Value, null));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);

            var testScores = new List<(byte, int, byte)>
            {
                (1, 1502, 0), (1, 1383, 0), (1, 1357, 0), (1, 1287, 0), (1, 1197, 0), (1, 739, 0),
                (0, 1775, 0), (0, 1653, 0), (0, 1239, 0), (0, 899, 0), (0, 496, 0), (0, 371, 0)
            };

            var realMMRChange = new List<double>
            {
                75, 75, 75, 75, 75, 75,
                -75, -75, -75, -75, -75, -75
            };

            var testMatch = MatchUtility.CreateBaseMatch(1, 3, testScores, _season, _region);

            //Act
            var testMMRChange = (await Task.WhenAll(testMatch.PlayerRecords.Select(pr => mmrService.CalculateMMRChangeAsync(pr)))).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }

        [Fact]
        public async Task CorrectMMRCalculation_NoCalibration31_MMRChangeIsTimes2()
        {
            //Arrange
            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new EnhancedCalibrationStrategy(_options.Value, null));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);

            var testScores = new List<(byte, int, byte)>
            {
                (1, 1502, 0), (1, 1383, 0), (1, 1357, 0), (1, 1287, 0), (1, 1197, 0), (1, 739, 0),
                (0, 1775, 0), (0, 1653, 0), (0, 1239, 0), (0, 899, 0), (0, 496, 0), (0, 371, 0)
            };

            var realMMRChange = new List<double>
            {
                50, 50, 50, 50, 50, 50,
                -50, -50, -50, -50, -50, -50
            };

            var testMatch = MatchUtility.CreateBaseMatch(1, 4, testScores, _season, _region);

            //Act
            var testMMRChange = (await Task.WhenAll(testMatch.PlayerRecords.Select(pr => mmrService.CalculateMMRChangeAsync(pr)))).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }

        [Fact]
        public async Task CorrectMMRCalculation_NoCalibration32_MMRChangeIsNotMultiplied()
        {
            //Arrange
            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new EnhancedCalibrationStrategy(_options.Value, null));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);

            var testScores = new List<(byte, int, byte)>
            {
                (1, 1502, 0), (1, 1383, 0), (1, 1357, 0), (1, 1287, 0), (1, 1197, 0), (1, 739, 0),
                (0, 1775, 0), (0, 1653, 0), (0, 1239, 0), (0, 899, 0), (0, 496, 0), (0, 371, 0)
            };

            var realMMRChange = new List<double>
            {
                25, 25, 25, 25, 25, 25,
                -25, -25, -25, -25, -25, -25
            };

            var testMatch = MatchUtility.CreateBaseMatch(1, 5, testScores, _season, _region);

            //Act
            var testMMRChange = (await Task.WhenAll(testMatch.PlayerRecords.Select(pr => mmrService.CalculateMMRChangeAsync(pr)))).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }

        [Fact]
        public async Task CorrectMMRCalculation_CalibrationWonInfOverMedian_MoreThan0()
        {
            //Arrange
            var testScores = new List<(byte, int, byte)>
            {
                (1, 1502, 1), (1, 1383, 0), (1, 1357, 0), (1, 1287, 0), (1, 1197, 0), (1, 739, 0),
                (0, 1775, 0), (0, 1653, 0), (0, 1239, 0), (0, 899, 0), (0, 496, 0), (0, 371, 0)
            };

            var testMatch = MatchUtility.CreateBaseMatch(1, 5, testScores, _season, _region);

            var testRecord = testMatch.PlayerRecords.Where(x => x.TeamIndex == x.Match.TeamWon).First();
            var testPlayer = testRecord.Player;
            testRecord.CalibrationIndex = 10;
            testPlayer.MainClass = Core.Domain.Player.PlayerClass.Infantry;

            var mediatrMock = new Mock<IMediator>();
            mediatrMock
                .Setup(m => m.Send(It.IsAny<GetPlayersAvgCalibrationScoreQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(200);

            mediatrMock
                .Setup(m => m.Send(It.IsAny<GetPlayerByIdQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testPlayer);

            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new EnhancedCalibrationStrategy(_options.Value, mediatrMock.Object));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);


            //Act
            var testMMRChange = await mmrService.CalculateMMRChangeAsync(testRecord);

            //Assert
            testMMRChange.Should().BeGreaterThan(0);
            Assert.Equal(158.47089761374281, testMMRChange);
        }

        [Fact]
        public async Task CorrectMMRCalculation_CalibrationWonInfOverMedian_ChangeMoreThan0()
        {
            //Arrange
            var testScores = new List<(byte, int, byte)>
            {
                (1, 1502, 1), (1, 1383, 0), (1, 1357, 0), (1, 1287, 0), (1, 1197, 0), (1, 739, 0),
                (0, 1775, 0), (0, 1653, 0), (0, 1239, 0), (0, 899, 0), (0, 496, 0), (0, 371, 0)
            };

            var testMatch = MatchUtility.CreateBaseMatch(1, 5, testScores, _season, _region);

            var testRecord = testMatch.PlayerRecords.Where(x => x.TeamIndex != x.Match.TeamWon).First();
            var testPlayer = testRecord.Player;
            testRecord.CalibrationIndex = 10;
            testPlayer.MainClass = Core.Domain.Player.PlayerClass.Infantry;

            var mediatrMock = new Mock<IMediator>();
            mediatrMock
                .Setup(m => m.Send(It.IsAny<GetPlayersAvgCalibrationScoreQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(200);

            mediatrMock
                .Setup(m => m.Send(It.IsAny<GetPlayerByIdQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testPlayer);

            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new EnhancedCalibrationStrategy(_options.Value, mediatrMock.Object));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);


            //Act
            var testMMRChange = await mmrService.CalculateMMRChangeAsync(testRecord);

            //Assert
            testMMRChange.Should().BeGreaterThan(0);
            Assert.Equal(58.470897613742821, testMMRChange);
        }

        [Fact]
        public async Task CorrectMMRCalculation_CalibrationWonInfBelowMedian_ChangeLessThan0()
        {
            //Arrange
            var testScores = new List<(byte, int, byte)>
            {
                (1, 1502, 1), (1, 1383, 0), (1, 1357, 0), (1, 1287, 0), (1, 1197, 0), (1, 739, 0),
                (0, 1775, 0), (0, 1653, 0), (0, 1239, 0), (0, 899, 0), (0, 496, 0), (0, 371, 0)
            };

            var testMatch = MatchUtility.CreateBaseMatch(1, 5, testScores, _season, _region);

            var testRecord = testMatch.PlayerRecords.Where(x => x.TeamIndex == x.Match.TeamWon).First();
            var testPlayer = testRecord.Player;
            testRecord.CalibrationIndex = 10;
            testPlayer.MainClass = Core.Domain.Player.PlayerClass.Infantry;

            var mediatrMock = new Mock<IMediator>();
            mediatrMock
                .Setup(m => m.Send(It.IsAny<GetPlayersAvgCalibrationScoreQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(100);

            mediatrMock
                .Setup(m => m.Send(It.IsAny<GetPlayerByIdQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testPlayer);

            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new EnhancedCalibrationStrategy(_options.Value, mediatrMock.Object));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);


            //Act
            var testMMRChange = await mmrService.CalculateMMRChangeAsync(testRecord);

            //Assert
            testMMRChange.Should().BeLessThan(0);
            Assert.Equal(-347.84411161636689, testMMRChange);
        }

        [Fact]
        public async Task CorrectMMRCalculation_CalibrationLostInfBeloMedian_ChangeIsAsCalculated()
        {
            //Arrange
            var testScores = new List<(byte, int, byte)>
            {
                (1, 1502, 1), (1, 1383, 0), (1, 1357, 0), (1, 1287, 0), (1, 1197, 0), (1, 739, 0),
                (0, 1775, 0), (0, 1653, 0), (0, 1239, 0), (0, 899, 0), (0, 496, 0), (0, 371, 0)
            };

            var testMatch = MatchUtility.CreateBaseMatch(1, 5, testScores, _season, _region);

            var testRecord = testMatch.PlayerRecords.Where(x => x.TeamIndex != x.Match.TeamWon).First();
            var testPlayer = testRecord.Player;
            testRecord.CalibrationIndex = 10;
            testPlayer.MainClass = Core.Domain.Player.PlayerClass.Infantry;

            var mediatrMock = new Mock<IMediator>();
            mediatrMock
                .Setup(m => m.Send(It.IsAny<GetPlayersAvgCalibrationScoreQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(150);

            mediatrMock
                .Setup(m => m.Send(It.IsAny<GetPlayerByIdQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testPlayer);

            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new EnhancedCalibrationStrategy(_options.Value, mediatrMock.Object));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);


            //Act
            var testMMRChange = await mmrService.CalculateMMRChangeAsync(testRecord);

            //Assert
            testMMRChange.Should().BeLessThan(0);
            Assert.Equal(-60.341763365451655, testMMRChange);
        }

        [Fact]
        public async Task CorrectMMRCalculation_AvgScoreMoreThan1000_MMRChangeCapped()
        {
            //Arrange
            var testScores = new List<(byte, int, byte)>
            {
                (1, 1502, 1), (1, 1383, 0), (1, 1357, 0), (1, 1287, 0), (1, 1197, 0), (1, 739, 0),
                (0, 1775, 0), (0, 1653, 0), (0, 1239, 0), (0, 899, 0), (0, 496, 0), (0, 371, 0)
            };

            var testMatch = MatchUtility.CreateBaseMatch(1, 5, testScores, _season, _region);

            var testRecord = testMatch.PlayerRecords.Where(x => x.TeamIndex != x.Match.TeamWon).First();
            var testPlayer = testRecord.Player;
            testRecord.CalibrationIndex = 10;
            testPlayer.MainClass = Core.Domain.Player.PlayerClass.Infantry;

            var mediatrMock = new Mock<IMediator>();
            mediatrMock
                .Setup(m => m.Send(It.IsAny<GetPlayersAvgCalibrationScoreQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1200);

            mediatrMock
                .Setup(m => m.Send(It.IsAny<GetPlayerByIdQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testPlayer);

            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new EnhancedCalibrationStrategy(_options.Value, mediatrMock.Object));

            var mmrService1200 = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);

            var mediatrMock2 = new Mock<IMediator>();
            mediatrMock2
                .Setup(m => m.Send(It.IsAny<GetPlayersAvgCalibrationScoreQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(10000);

            mediatrMock2
                .Setup(m => m.Send(It.IsAny<GetPlayerByIdQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testPlayer);

            var builderMock2 = new Mock<IMMRCalculationBuilder>();
            builderMock2.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new EnhancedCalibrationStrategy(_options.Value, mediatrMock2.Object));

            var mmrService10000 = new MMRCalculationService(_options, _seasonResolver, builderMock2.Object);

            //Act
            var testMMRChange1200 = await mmrService1200.CalculateMMRChangeAsync(testRecord);
            var testMMRChange10000 = await mmrService10000.CalculateMMRChangeAsync(testRecord);

            //Assert
            testMMRChange1200.Should().BeGreaterThan(0);
            testMMRChange10000.Should().BeGreaterThan(0);
            testMMRChange1200.Should().NotBe(0);
            testMMRChange10000.Should().NotBe(0);

            Assert.Equal(testMMRChange1200, testMMRChange10000);
        }
    }
}
