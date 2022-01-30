using BL.API.Core.Domain.Match;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.API.Services.Matches.Queries
{
    public class MatchResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string ScreenshotLink { get; set; }
        public byte TeamWon { get; set; }
        public byte RoundsPlayed { get; set; }
        public IEnumerable<PlayerMatchResponse> PlayerRecords { get; set; }

        public static MatchResponse FromMatch(Match match)
        {
            return new MatchResponse 
            {
                Id = match.Id,
                Date = match.MatchDate,
                RoundsPlayed = match.RoundsPlayed,
                ScreenshotLink = match.ScreenshotLink,
                TeamWon = match.TeamWon,
                PlayerRecords = match.PlayerRecords.Select(x => PlayerMatchResponse.FromPlayerMatchRecord(x)).ToList()
            };

        }
    }

    public class PlayerMatchResponse
    {
        public Guid Id { get; set; }
        public Guid? PlayerId { get; set; }
        public byte TeamNumber { get; set; }
        public byte RoundsWon { get; set; }
        public string Faction { get; set; }
        public sbyte? Kills { get; set; }
        public sbyte? Assists { get; set; }
        public int? Score { get; set; }
        public byte? MVPs { get; set; }
        public double? MMRChange { get; set; }
        public byte? CalibrationIndex { get; set; }

        public static PlayerMatchResponse FromPlayerMatchRecord(PlayerMatchRecord matchRecord)
        {
            return new PlayerMatchResponse 
            {
                Id = matchRecord.Id,
                PlayerId = matchRecord.PlayerId,
                TeamNumber = matchRecord.TeamIndex,
                RoundsWon = matchRecord.RoundsWon,
                Faction = matchRecord.Faction?.ToString(),
                Kills = matchRecord.Kills,
                Assists = matchRecord.Assists,
                Score = matchRecord.Score,
                MVPs = matchRecord.MVPs,
                MMRChange = matchRecord.MMRChange,
                CalibrationIndex = matchRecord.CalibrationIndex
            };
        }
    }
}
