using BL.API.Core.Domain.User;
using BL.API.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Authorization.Commands
{
    public class AddRoleToUserCommand : IRequest<Task>
    {
        public string TargetUserName { get; set; }
        public string TargetRole { get; set; }

        public class AddRoleToUserCommandHandler : IRequestHandler<AddRoleToUserCommand, Task>
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public AddRoleToUserCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<Task> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.TargetUserName);
                
                if (user == null)
                    throw new NotFoundException();

                var role = request.TargetRole;

                if (role != UserRoles.Admin && role != UserRoles.Moderator
                    && role != UserRoles.MatchMaker && role != UserRoles.User)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));

                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }

                return Task.CompletedTask;
            }
        }
    }
}
