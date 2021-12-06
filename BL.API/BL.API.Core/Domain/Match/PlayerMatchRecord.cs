using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.API.Core.Domain.Match
{
    public class PlayerMatchRecord : BaseEntity
    {
        public Guid PlayerId { get; protected set; }
        [ForeignKey("PlayerId")]
        public virtual Player.Player Player { get; protected set; }
        public Guid MatchId { get; protected set; }
        public virtual Match Match { get; protected set; }
        public int RoundsPlayed { get; protected set; }
        public Faction Faction { get; protected set; }
        public int Kills { get; protected set; }
        public int Assists { get; protected set; }
        public int? Deaths { get; protected set; }
        public int Score { get; protected set; }
        public int MVPs { get; protected set; }
        public int MMRChange { get; protected set; }
    }
}
