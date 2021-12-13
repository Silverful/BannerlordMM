using System.ComponentModel.DataAnnotations;

namespace BL.API.WebHost.Model
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
