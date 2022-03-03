using System.ComponentModel.DataAnnotations;

namespace BL.API.Core.Domain.Settings
{
    public class Region : BaseEntity
    {
        [MaxLength(8)]
        [Required(AllowEmptyStrings = false)]
        public string ShortName { get; set; }
        [MaxLength(32)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
    }
}
