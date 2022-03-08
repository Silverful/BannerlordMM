using BL.API.Core.Domain.User;
using BL.API.Core.Exceptions;
using BL.API.Services.Authorization.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Authorization.Commands
{
    public class RegisterAdminCommand : RegisterRequest 
    {
        public class RegisterAdminCommandHadler : IRequestHandler<RegisterAdminCommand, Task>
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<Role> _roleManager;

            public RegisterAdminCommandHadler(UserManager<User> userManager, RoleManager<Role> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<Task> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
            {
                var userExists = await _userManager.FindByNameAsync(request.Username);
                if (userExists != null)
                    throw new AlreadyExistsException();

                User user = new User()
                {
                    Email = request.Email,
                    UserName = request.Username
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                    throw new Exception(string.Join('/', result.Errors.Select(x => x.Description)));

                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _roleManager.CreateAsync(new Role(UserRoles.Admin));

                if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                    await _roleManager.CreateAsync(new Role(UserRoles.User));

                if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                }

                return Task.CompletedTask;
            }
        }
    }
}
