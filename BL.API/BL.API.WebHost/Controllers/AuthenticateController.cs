using BL.API.Core.Domain.User;
using BL.API.WebHost.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BL.API.WebHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        //private readonly UserManager<User> userManager;
        //private readonly RoleManager<UserRole> roleManager;
        //private readonly IConfiguration _configuration;
        //public AuthenticateController(UserManager<User> userManager, RoleManager<UserRole> roleManager, IConfiguration configuration)
        //{
        //    this.userManager = userManager;
        //    this.roleManager = roleManager;
        //    _configuration = configuration;
        //}

        //[HttpPost]
        //[Route("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        //{
        //    var userExists = await userManager.FindByNameAsync(request.UserName);
        //    if (userExists != null)
        //        return BadRequest("User already exists");

        //    User user = new User()
        //    {
        //        Email = request.Email,
        //        UserName = request.UserName
        //    };
        //    var result = await userManager.CreateAsync(user, request.Password);

        //    if (!result.Succeeded)
        //        return BadRequest("User creation failed");
            
        //    return Ok();
        //}

        //[HttpPost]
        //[Route("registeradmin")]
        //public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequest request)
        //{
        //    var userExists = await userManager.FindByNameAsync(request.UserName);
        //    if (userExists != null)
        //        return BadRequest("User already exists");

        //    User user = new User()
        //    {
        //        Email = request.Email,
        //        UserName = request.UserName
        //    };
        //    var result = await userManager.CreateAsync(user, request.Password);
        //    if (!result.Succeeded)
        //        return BadRequest("User creation failed");

        //    (!await roleManager.RoleExistsAsync(UserRoles.Admin))
        //        await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        //    if (!await roleManager.RoleExistsAsync(UserRoles.User))
        //        await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
        //    if (await roleManager.RoleExistsAsync(UserRoles.Admin))
        //    {
        //        await userManager.AddToRoleAsync(user, UserRoles.Admin);
        //    }
        //    return Ok();
        //}


        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequest request)
        //{
        //    var user = await userManager.FindByNameAsync(request.UserName);
        //    if (user != null && await userManager.CheckPasswordAsync(user, request.Password))
        //    {
        //        var userRoles = await userManager.GetRolesAsync(user);
        //        var authClaims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, user.UserName),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        };

        //        foreach (var userRole in userRoles)
        //        {
        //            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        //        }

        //        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:SecretKey")));
        //        var token = new JwtSecurityToken(
        //        issuer: _configuration["JWT: ValidIssuer"],
        //        audience: _configuration["JWT: ValidAudience"],
        //        expires: DateTime.Now.AddHours(6),
        //        claims: authClaims,
        //        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //        );

        //        return Ok(new
        //        {
        //            token = new JwtSecurityTokenHandler().WriteToken(token),
        //            expiration = token.ValidTo
        //        });
        //    }

        //    return Unauthorized();
        //}
    }
}
