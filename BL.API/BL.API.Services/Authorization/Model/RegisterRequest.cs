using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BL.API.Services.Authorization.Model
{
    public class RegisterRequest : IRequest<Task>
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
