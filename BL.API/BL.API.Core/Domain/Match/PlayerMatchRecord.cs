using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.API.Core.Domain.Match
{
    public class PlayerMatchRecord : BaseEntity
    {
        public Guid PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public virtual Player.Player Player { get; set; }
        public Guid MatchId { get; set; }
        public virtual Match Match { get; set; }
        public Faction Faction { get; set; }
        public int Kills { get; set; }
        public int Assists { get; set; }
        public int? Deaths { get; set; }
        public int Score { get; set; }
        public int MVPs { get; set; }
        public int MMRChange { get; set; }
    }
}
