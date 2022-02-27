using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.API.Core.Domain.Settings
{
    public class Configuration : BaseEntity
    {
        [Required(AllowEmptyStrings = false)]
        public string ConfigName { get; set; }
        public string Value { get; set; }
        public Guid? RegionId { get; set; }
        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }
    }
}
