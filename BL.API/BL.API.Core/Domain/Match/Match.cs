using System.Collections.Generic;

namespace BL.API.Core.Domain.Match
{
    public class Match : BaseEntity
    {
        public string ScreenshotLink { get; protected set; }
        public Faction FactionWon { get; protected set; }
        public int RoundsPlayed { get; protected set; }
        public virtual ICollection<TeamPlayer> Players { get; protected set; }
        public virtual ICollection<PlayerMatchRecord> PlayerRecords { get; protected set; }
    }
}
