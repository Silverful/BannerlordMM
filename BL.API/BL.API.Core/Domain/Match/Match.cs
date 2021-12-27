using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.API.Core.Domain.Match
{
    public class Match : BaseEntity
    {
        [Column(TypeName = "varchar(200)")]
        [Required(AllowEmptyStrings = true)]
        public string ScreenshotLink { get; set; }
        public DateTime MatchDate { get; set; }
        public byte RoundsPlayed { get; set; }
        public byte TeamWon { get; set; }
        public Guid? SeasonId { get; set; }
        public virtual Season Season { get; set; }
        public virtual ICollection<PlayerMatchRecord> PlayerRecords { get; set; }
    }
}
