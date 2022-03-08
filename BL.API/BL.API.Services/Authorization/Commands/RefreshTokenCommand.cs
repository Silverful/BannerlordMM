using BL.API.Services.Authorization.Model;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Authorization.Commands
{
    public class RefreshTokenCommand : IRequest<SignInResult>
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }

        public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, SignInResult>
        {
            private readonly JWTSignInManager _signInManager;

            public RefreshTokenCommandHandler(JWTSignInManager signInManager)
            {
                _signInManager = signInManager;
            }

            public async Task<SignInResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            {
                return await _signInManager.RefreshTokenAsync(request.AccessToken, request.RefreshToken);
            }
        }
    }
}
