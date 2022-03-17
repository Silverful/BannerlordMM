using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Settings;
using BL.API.Services.MMR;
using BL.API.UnitTests.Builders;
using BL.API.UnitTests.Utility;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace BL.API.UnitTests.Services.MMR
{
    public class BasicMMRCalculationAsyncTests
    {
        private readonly IOptions<BasicMMRCalculationProperties> _options;
        private readonly Region _region;
        private readonly Season _season;
        private readonly ISeasonResolverService _seasonResolver;

        public BasicMMRCalculationAsyncTests()
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
                .WithTitle("Basic")
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
        public async Task CorrectMMRCalculation_NoCalibration_ArraysIdentical()
        {
            //Arrange
            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new BasicStrategy(_options.Value));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);

            var testScores = new List<(byte, int, byte)>
            {
                (1, 1502, 0), (1, 1383, 0), (1, 1357, 0), (1, 1287, 0), (1, 1197, 0), (1, 739, 0),
                (0, 1775, 0), (0, 1653, 0), (0, 1239, 0), (0, 899, 0), (0, 496, 0), (0, 371, 0)
            };

            var realMMRChange = new List<double>
            {
                20, 20, 20, 20, 20, 20,
                -20, -20, -20, -20, -20, -20
            };

            var testMatch = MatchUtility.CreateBaseMatch(1, 5, testScores, _season, _region);

            //Act
            var testMMRChange = (await Task.WhenAll(testMatch.PlayerRecords.Select(pr => mmrService.CalculateMMRChangeAsync(pr)))).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }

        [Fact]
        public async Task CorrectMMRCalculation_WithCalibration_ArraysIdentical()
        {
            //Arrange
            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new BasicStrategy(_options.Value));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);

            var testScores = new List<(byte, int, byte)>
            {
                (1, 1502, 2), (1, 1383, 3), (1, 1357, 5), (1, 1287, 1), (1, 1197, 6), (1, 739, 6),
                (0, 1775, 8), (0, 1653, 7), (0, 1239, 6), (0, 899, 1), (0, 496, 2), (0, 371, 3)
            };

            var realMMRChange = new List<double>
            {
                80, 80, 80, 80, 80, 80,
                0, 0, 0, 0, 0, 0
            };

            var testMatch = MatchUtility.CreateBaseMatch(1, 5, testScores, _season, _region);

            //Act
            var testMMRChange = (await Task.WhenAll(testMatch.PlayerRecords.Select(pr => mmrService.CalculateMMRChangeAsync(pr)))).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }

        [Fact]
        public async Task CorrectMMRCalculation_WithAndWithoutCalibration_ArraysIdentical()
        {
            //Arrange
            var builderMock = new Mock<IMMRCalculationBuilder>();
            builderMock.Setup(b => b.BuildMMRStrategy(It.IsAny<Season>(), It.IsAny<BasicMMRCalculationProperties>()))
                .Returns(new BasicStrategy(_options.Value));

            var mmrService = new MMRCalculationService(_options, _seasonResolver, builderMock.Object);

            var testScores = new List<(byte, int, byte)>
            {
                (1, 1502, 0), (1, 1383, 3), (1, 1357, 0), (1, 1287, 1), (1, 1197, 6), (1, 739, 6),
                (0, 1775, 0), (0, 1653, 7), (0, 1239, 0), (0, 899, 1), (0, 496, 2), (0, 371, 3)
            };

            var realMMRChange = new List<double>
            {
                20, 80, 20, 80, 80, 80,
                -20, 0, -20, 0, 0, 0
            };

            var testMatch = MatchUtility.CreateBaseMatch(1, 5, testScores, _season, _region);

            //Act
            var testMMRChange = (await Task.WhenAll(testMatch.PlayerRecords.Select(pr => mmrService.CalculateMMRChangeAsync(pr)))).ToList();

            //Assert
            int i = 0;
            testMMRChange.ForEach(t => Assert.Equal(realMMRChange[i++], t));
        }
    }
}
