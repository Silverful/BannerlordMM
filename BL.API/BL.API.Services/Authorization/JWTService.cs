using BL.API.Services.Authorization.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BL.API.Services.Authorization
{
    public class JWTService
    {
        private readonly JWTConfiguration _configuration;
        private readonly ILogger<JWTService> _logger;

        public JWTService(IOptions<JWTConfiguration> configuration,
                              ILogger<JWTService> logger)
        {
            _configuration = configuration.Value;
            _logger = logger;
        }

        public string BuildToken(Claim[] claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.SecretKey));
            var creds = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.ValidIssuer,
                audience: _configuration.ValidAudience,
                expires: DateTime.Now.AddMinutes(_configuration.TokenExpiration),
                notBefore: DateTime.Now,
                claims: claims,
                signingCredentials: creds
            ); ;

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string BuildRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            JwtSecurityTokenHandler tokenValidator = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.SecretKey));

            var parameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = false
            };

            try
            {
                var principal = tokenValidator.ValidateToken(token, parameters, out var securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogError($"Token validation failed");
                    return null;
                }

                return principal;
            }
            catch (Exception e)
            {
                _logger.LogError($"Token validation failed: {e.Message}");
                return null;
            }
        }
    }
}

