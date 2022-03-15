using BL.API.Core.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BL.API.Services.Authorization
{
    public class AuthSeeder
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthSeeder> _logger;

        public AuthSeeder(UserManager<User> userManager, 
            RoleManager<Role> roleManager,
            IConfiguration configuration,
            ILogger<AuthSeeder> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task Seed()
        {
            try
            {
                await SeedRoles();
                await SeedSuperAdminAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Seeding went wrong");
            }
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
