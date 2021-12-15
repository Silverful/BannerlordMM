using BL.API.Core.Domain.Match;
using System;
using System.Collections.Generic;

namespace BL.API.UnitTests.Builders
{
    public class MatchBuilder
    {
        private Guid _id;
        private string _screenshotLink;
        private DateTime _matchDate;
        private byte _roundsPlayed;
        private byte _teamWon;
        private ICollection<PlayerMatchRecord> _playerRecords;

        public MatchBuilder WithId(Guid? id)
        {
            _id = id ?? Guid.NewGuid();
            return this;
        }

        public MatchBuilder WithScreenshotLink(string link)
        {
            _screenshotLink = link;
            return this;
        }

        public MatchBuilder WithMatchDate(DateTime matchDate)
        {
            _matchDate = matchDate;
            return this;
        }

        public MatchBuilder WithRoundsPlayed(byte roundsPlayed)
        {
            _roundsPlayed = roundsPlayed;
            return this;
        }

        public MatchBuilder WithTeamWon(byte teamWon)
        {
            _teamWon = teamWon;
            return this;
        }

        public MatchBuilder WithPlayerRecords(ICollection<PlayerMatchRecord> playerRecords)
        {
            _playerRecords = playerRecords;
            return this;
        }

        public Match Build()
        {
            return new Match
            {
                Id = _id,
                MatchDate = _matchDate,
                RoundsPlayed = _roundsPlayed,
                TeamWon = _teamWon,
                ScreenshotLink = _screenshotLink,
                PlayerRecords = _playerRecords
            };
        }
    }
}
