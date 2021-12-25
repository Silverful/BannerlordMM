using BL.API.Core.Domain.Match;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.API.Core.Domain.Player
{
    public class PlayerMMR : BaseEntity
    {
        public int MMR { get; set; }

        public Guid SeasonId { get; set; }
        [ForeignKey("SeasonId")]
        public Season Season { get; set; }

        public Guid PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public virtual Player Player {get;set;}
    }
}
