using BL.API.Core.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BL.API.Services.Authorization
{
    public class AuthSeeder
    {
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;
        private IConfiguration _configuration;

        public AuthSeeder(UserManager<User> userManager, RoleManager<Role> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            await SeedRoles();
            await SeedSuperAdminAsync();
        }

        private async Task SeedRoles()
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new Role(UserRoles.Admin));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.MatchMaker))
            {
                await _roleManager.CreateAsync(new Role(UserRoles.MatchMaker));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Moderator))
            {
                await _roleManager.CreateAsync(new Role(UserRoles.Moderator));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new Role(UserRoles.User));
            }
        }

        private async Task SeedSuperAdminAsync()
        {
            var email = _configuration["Auth:SAEmail"];
            var password = _configuration["Auth:SAPassword"];

            var user = await _userManager.FindByNameAsync(email);

            if (user == null)
            {
                var admin = new User
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(admin, password);

                await _userManager.AddToRoleAsync(admin, UserRoles.Admin);
            }
        }
    }
}
