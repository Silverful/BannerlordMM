using System;
using System.Collections.Generic;

namespace BL.API.Core.Domain.Match
{
    public class Match : BaseEntity
    {
        public string ScreenshotLink { get; set; }
        public DateTime MatchDate { get; set; }
        public byte RoundsPlayed { get; set; }
        public byte TeamWon { get; set; }
        public virtual ICollection<PlayerMatchRecord> PlayerRecords { get; set; }
    }
}
