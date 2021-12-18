using AutoFixture;
using AutoFixture.AutoMoq;
using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Services.Matches.Commands;
using BL.API.Services.MMR;
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
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static BL.API.Services.Matches.Commands.UploadMatchCommand;

namespace BL.API.UnitTests.WebHost.Matches.Commands
{
    public class UploadMatchAsyncTests
    {
        private readonly Mock<IRepository<Player>> _playersMoq;
        private readonly Mock<IRepository<PlayerMatchRecord>> _matchRecords;
        private readonly Mock<IRepository<Core.Domain.Match.Match>> _matchMoq;
        private readonly Mock<IMediator> _mediatr;
        private readonly MMRCalculationService _mmrCalcService;

        public UploadMatchAsyncTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _playersMoq = fixture.Freeze<Mock<IRepository<Player>>>();
            _matchRecords = fixture.Freeze<Mock<IRepository<PlayerMatchRecord>>>();
            _matchMoq = fixture.Freeze<Mock<IRepository<Core.Domain.Match.Match>>>();
            _mediatr = new Mock<IMediator>();

            var options = Options.Create(new BasicMMRCalculationProperties
            {
                DefaultChange = 20,
                AdditionalBank = 40
            });

            _mmrCalcService = new MMRCalculationService(options);
        }

        public IEnumerable<Player> CreatePlayers(int number)
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
                .Generate(number)
                .ToList();

            return players;
        }

        public UploadMatchCommand CreateMatchCommand(IList<Player> players)
        {
            var rnd = new Random();

            byte roundsWon = (byte)rnd.Next(3, 5);
            byte roundsLost = (byte)rnd.Next(0, roundsWon - 1);
            var faction1 = (Faction)rnd.Next(1, 6);
            var faction2 = (Faction)rnd.Next(1, 6);

            var faker = new Faker<MatchRecord>()
                .RuleFor(p => p.Kills, f => f.Random.SByte(-2, 20))
                .RuleFor(p => p.Assists, f => f.Random.SByte(-2, 20))
                .RuleFor(p => p.Score, f => f.Random.Int(-100, 2000))
                .RuleFor(p => p.MVPs, f => f.Random.Byte(0, 5));

            var team1 = faker.Generate(6);

            int i = 0;
            team1.ForEach(r => 
            {
                r.PlayerId = players[i].Id;
                r.RoundsWon = roundsWon;
                r.Faction = faction1.ToString();
                i++;
            });

            var team2 = faker.Generate(6);

            team2.ForEach(r =>
            {
                r.PlayerId = players[i].Id;
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
                .Generate();
        }


        [Fact]
        public async Task UploadMatch_CorrectMatch_ReturnsOk()
        {
            //Arrange
            var players = CreatePlayers(12).ToList();
            var matchCommand = CreateMatchCommand(players);

            _playersMoq.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(players);

            Core.Domain.Match.Match nullObj = null;

            _matchMoq.Setup(repo => repo.GetFirstWhereAsync(m => m.ScreenshotLink == matchCommand.ScreenshotLink))
                .ReturnsAsync(nullObj);

            _matchMoq.Setup(repo => repo.CreateAsync(It.IsAny<Core.Domain.Match.Match>()))
                .ReturnsAsync(It.Is<Guid>(g => g != Guid.Empty));

            var handler = new UploadMatchCommandHandler(_matchMoq.Object, _matchRecords.Object, _playersMoq.Object, _mmrCalcService, null);

            //Act
            var result = await handler.Handle(matchCommand, new System.Threading.CancellationToken());

            //Assert
            result.Should().BeEmpty();
        }
    }
}
