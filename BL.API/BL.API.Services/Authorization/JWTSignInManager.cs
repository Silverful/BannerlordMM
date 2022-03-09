using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.User;
using BL.API.Services.Authorization.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SignInResult = BL.API.Services.Authorization.Model.SignInResult;

namespace BL.API.Services.Authorization
{
    public class JWTSignInManager
    {
        private readonly ILogger<JWTSignInManager> _logger;
        private readonly JWTService _JwtAuthService;
        private readonly JWTConfiguration _configuration;
        private readonly IRepository<RefreshToken> _refreshTokens;
        private readonly UserManager<User> _userManager;

        public JWTSignInManager(ILogger<JWTSignInManager> logger,
                             JWTService JWTAuthService,
                             IOptions<JWTConfiguration> configuration,
                             IRepository<RefreshToken> refreshTokens,
                             UserManager<User> userManager)
        {
            _logger = logger;
            _refreshTokens = refreshTokens;
            _JwtAuthService = JWTAuthService;
            _configuration = configuration.Value;
            _userManager = userManager;
        }

        public async Task<SignInResult> SignInAsync(string username, string password)
        {
            _logger.LogInformation($"Validating user [{username}]", username);

            SignInResult result = new SignInResult();

            if (string.IsNullOrWhiteSpace(username)) return result;
            if (string.IsNullOrWhiteSpace(password)) return result;

            var user = await _userManager.FindByNameAsync(username);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {

                var claims = BuildClaims(user);
                result.User = user;
                result.AccessToken = _JwtAuthService.BuildToken(claims);
                result.RefreshToken = _JwtAuthService.BuildRefreshToken();

                await _refreshTokens.CreateAsync(new RefreshToken { UserId = user.Id, Token = result.RefreshToken, IssuedAt = DateTime.Now, ExpiresAt = DateTime.Now.AddMinutes(_configuration.RefreshTokenExpiration) });

                result.Success = true;
            };

            return result;
        }

        public async Task<SignInResult> RefreshTokenAsync(string AccessToken, string RefreshToken)
        {
            ClaimsPrincipal claimsPrincipal = _JwtAuthService.GetPrincipalFromToken(AccessToken);
            SignInResult result = new SignInResult();

            if (claimsPrincipal == null) return result;

            string id = claimsPrincipal.Claims.First(c => c.Type == "id").Value;
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return result;

            var token = await _refreshTokens.GetFirstWhereAsync(
                    f => f.UserId == user.Id
                            && f.Token == RefreshToken
                            && f.ExpiresAt >= DateTime.Now, true);

            if (token == null) return result;

            var claims = BuildClaims(user);

            result.User = user;
            result.AccessToken = _JwtAuthService.BuildToken(claims);
            result.RefreshToken = _JwtAuthService.BuildRefreshToken();

            await _refreshTokens.DeleteAsync(token);
            await _refreshTokens.CreateAsync(new RefreshToken { UserId = user.Id, Token = result.RefreshToken, IssuedAt = DateTime.Now, ExpiresAt = DateTime.Now.AddMinutes(_configuration.RefreshTokenExpiration) });

            result.Success = true;

            return result;
        }

        private Claim[] BuildClaims(User user)
        {
            var role = _userManager.GetRolesAsync(user).Result.First();
            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            return claims;
        }
    }
}
