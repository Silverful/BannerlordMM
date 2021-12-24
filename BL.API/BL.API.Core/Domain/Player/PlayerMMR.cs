using BL.API.Core.Domain.Match;
using System;

namespace BL.API.Core.Domain.Player
{
    public class PlayerMMR : BaseEntity
    {
        public int MMR { get; set; }
        public Guid SeasonId { get; set; }
        public Season Season { get; set; }
        public Guid PlayerId { get; set; }
        public virtual Player Player {get;set;}
    }
}
