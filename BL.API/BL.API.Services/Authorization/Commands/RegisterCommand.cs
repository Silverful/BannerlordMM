using BL.API.Core.Domain.User;
using BL.API.Core.Exceptions;
using BL.API.Services.Authorization.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Authorization.Commands
{
    public class RegisterCommand : RegisterRequest
    {
        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Task>
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public RegisterCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<Task> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                var userExists = await _userManager.FindByNameAsync(request.Username);

                if (userExists != null)
                    throw new AlreadyExistsException();

                User user = new User()
                {
                    Email = request.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = request.Username
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                    throw new Exception("Something went wrong");

                if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                if (await _roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                }

                return Task.CompletedTask;
            }
        }
    }
}
