using BL.API.Services.Authorization.Model;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Authorization.Commands
{
    public class LoginCommand : IRequest<SignInResult>
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }


        public class LoginCommandHandler : IRequestHandler<LoginCommand, SignInResult>
        {
            private readonly JWTSignInManager _signInManager;

            public LoginCommandHandler(JWTSignInManager signInManager)
            {
                _signInManager = signInManager;
            }

            public async Task<SignInResult> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                return await _signInManager.SignInAsync(request.Username, request.Password);
            }
        }
    }
}
