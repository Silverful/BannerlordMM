using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.API.Core.Domain.Match
{
    public class PlayerMatchRecord : BaseEntity
    {
        public Guid? PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public virtual Player.Player Player { get; set; }
        public Guid MatchId { get; set; }
        [ForeignKey("MatchId")]
        public virtual Match Match { get; set; }
        public byte TeamIndex { get; set; }
        public byte RoundsWon { get; set; }
        public Faction? Faction { get; set; }
        public sbyte? Kills { get; set; }
        public sbyte? Assists { get; set; }
        public byte? Deaths { get; set; }
        public int? Score { get; set; }
        public byte? MVPs { get; set; }
        public double? MMRChange { get; set; }
        public byte? CalibrationIndex { get; set; }
    }
}
