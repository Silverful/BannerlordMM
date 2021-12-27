using BL.API.Core.Domain.Match;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.API.Services.Matches.Commands
{
    public class UploadMatchRequest
    {
        [Required]
        public string ScreenshotLink { get; set; }
        [Required]
        public DateTime MatchDate { get; set; }
        [Required, Range(3, 5)]
        public byte RoundsPlayed { get; set; }
        [Required, MaxLength(6)]
        public List<MatchRecord> Team1Records { get; set; }
        [Required, MaxLength(6)]
        public List<MatchRecord> Team2Records { get; set; }

        public class MatchRecord
        {
            public Guid? PlayerId { get; set; }
            public byte RoundsWon { get; set; }
            public string Faction { get; set; }
            public sbyte? Kills { get; set; }
            public sbyte? Assists { get; set; }
            public int? Score { get; set; }
            [Range(0, 5)]
            public byte? MVPs { get; set; }

            public PlayerMatchRecord ToPlayerMatchRecord(byte teamIndex)
            {
                Faction? faction = null;

                if (this.Faction != null)
                {
                    faction = (Faction)Enum.Parse(typeof(Faction), this.Faction);
                };

                return new PlayerMatchRecord
                {
                    PlayerId = this.PlayerId,
                    TeamIndex = teamIndex,
                    RoundsWon = this.RoundsWon,
                    Faction = faction,
                    Kills = this.Kills,
                    Assists = this.Assists,
                    Score = this.Score,
                    MVPs = this.MVPs
                };
            }
        }
    }
}
