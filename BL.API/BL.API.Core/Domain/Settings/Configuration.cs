using System.ComponentModel.DataAnnotations;

namespace BL.API.Core.Domain.Settings
{
    public class Configuration : BaseEntity
    {
        [Required(AllowEmptyStrings = false)]
        public string ConfigName { get; set; }
        public string Value { get; set; }
    }
}
