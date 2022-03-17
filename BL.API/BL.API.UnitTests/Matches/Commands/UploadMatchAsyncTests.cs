using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Domain.Settings;
using BL.API.Services.Matches.Commands;
using BL.API.Services.MMR;
using BL.API.UnitTests.Builders;
using BL.API.UnitTests.Utility;
using BL.API.WebHost.Controllers;
using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static BL.API.Services.Matches.Commands.LoadPlayersRecordCommand;
using static BL.API.Services.Matches.Commands.UploadMatchCommand;
using Match = BL.API.Core.Domain.Match.Match;

namespace BL.API.UnitTests.WebHost.Matches.Commands
{
    public class UploadMatchAsyncTests
    {
        private readonly IOptions<BasicMMRCalculationProperties> _options;
        private readonly Region _region;
        private readonly Season _season;
        private readonly ISeasonResolverService _seasonResolver;
        private readonly IRepository<Region> _regionRepository;

        public UploadMatchAsyncTests()
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

            var options = Options.Create(new BasicMMRCalculationProperties
            {
                DefaultChange = 20,
                AdditionalBank = 40
            });

            var seasonResolverMock = new Mock<ISeasonResolverService>();
            seasonResolverMock
                .Setup(r => r.GetCurrentSeasonAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_season);
            seasonResolverMock
                .Setup(r => r.GetSeasonOnDateAsync(It.IsAny<DateTime>(), It.IsAny<Guid>()))
                .ReturnsAsync(_season);

            _seasonResolver = seasonResolverMock.Object;

            var regionMock = new Mock<IRepository<Region>>();
            regionMock
                .Setup(r => r.GetFirstWhereAsync(It.IsAny<Expression<Func<Region, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Region, object>>>()))
                .ReturnsAsync(_region);

            _regionRepository = regionMock.Object;
        }


        [Fact]
        public async Task UploadMatch_CorrectMatch_ReturnsOk()
        {
            ////Arrange
            //var fixture = new Fixture().Customize(new AutoMoqCustomization());
            //var loggerMock = fixture.Freeze<Mock<ILogger<UploadMatchCommandHandler>>>();
            //var cacheMock = fixture.Freeze<Mock<ICacheProvider>>();

            //var players = MatchUtility.CreatePlayers(12, _season, _region).ToList();
            //var matchCommand = MatchUtility.CreateMatchCommand(_region);

            //var playersMock = new Mock<IRepository<Player>>();
            //playersMock.Setup(repo => repo.GetAllAsync(It.IsAny<bool>(), It.IsAny<Expression<Func<Player, object>>>()))
            //    .ReturnsAsync(players);

            //Core.Domain.Match.Match nullObj = null;

            //var matchMock = new Mock<IRepository<Match>>();
            //matchMock.Setup(repo => repo.GetFirstWhereAsync(m => m.ScreenshotLink == matchCommand.ScreenshotLink, 
            //    It.IsAny<bool>(), 
            //    It.IsAny<Expression<Func<Match, object>>>()))
            //    .ReturnsAsync(nullObj);

            //matchMock.Setup(repo => repo.CreateAsync(It.IsAny<Match>()))
            //    .ReturnsAsync(It.Is<Guid>(g => g != Guid.Empty));

            //var mock

            //var reloadHandler = new LoadPlayersRecordCommandHandler()

            //var mediator = new Mock<IMediator>();
            //mediator.Setup

            //var handler = new UploadMatchCommandHandler(matchMock.Object, _regionRepository, _seasonResolver, null, cacheMock.Object, loggerMock.Object);

            ////Act
            //var result = await handler.Handle(matchCommand, new System.Threading.CancellationToken());

            ////Assert
            //result.Should().BeEmpty();
        }

        //[Fact]
        //public async Task UploadMatch_NoPlayerIds_ReturnsOk()
        //{
        //    //Arrange
        //    var matchCommand = CreateMatchCommand(null);

        //    Core.Domain.Match.Match nullObj = null;

        //    _matchMoq.Setup(repo => repo.GetFirstWhereAsync(m => m.ScreenshotLink == matchCommand.ScreenshotLink))
        //        .ReturnsAsync(nullObj);

        //    _matchMoq.Setup(repo => repo.CreateAsync(It.IsAny<Core.Domain.Match.Match>()))
        //        .ReturnsAsync(It.Is<Guid>(g => g != Guid.Empty));

        //    var handler = new UploadMatchCommandHandler(_matchMoq.Object, _matchRecords.Object, _playersMoq.Object, _mmrCalcService, null);

        //    //Act
        //    var result = await handler.Handle(matchCommand, new System.Threading.CancellationToken());

        //    //Assert
        //    result.Should().BeEmpty();
        //}
    }
}
