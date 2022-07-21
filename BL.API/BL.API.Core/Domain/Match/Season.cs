using BL.API.Core.Domain.Settings;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.API.Core.Domain.Match
{
    public class Season : BaseEntity
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public bool OnGoing { get; set; }
        public bool IsTestingSeason { get; set; }
        public MMRAlgorithm? MMRAlgorithm { get; set; } 
        public DateTime? Started { get; set; }
        public DateTime? Finished { get; set; }
        public Guid? RegionId { get; set; }
        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }
    }
}
