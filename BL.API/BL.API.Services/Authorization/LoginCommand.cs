using BL.API.Core.Domain.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Authorization
{
    public class LoginCommand : IRequest<User>
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }


        public class LoginCommandHandler : IRequestHandler<LoginCommand, User>
        {
            private readonly UserManager<User> _userManager;

            private readonly SignInManager<User> _signInManager;

            public LoginCommandHandler(UserManager<User> userManager,
                                           SignInManager<User> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<User> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    return new UserModel
                    {
                        Token = "test",                      
                        UserName = user.UserName,
                        Image = null
                    };
                }

                throw new RestException(HttpStatusCode.Unauthorized);
            }
        }

    }
}
