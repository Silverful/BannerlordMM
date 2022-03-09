using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BL.API.WebHost.Middleware
{
    public class JwtAuthorizationMiddleware : IdentityUser
    {
        private readonly RequestDelegate _next;

        public JwtAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger logger)
        {
            var token = httpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                httpContext.Request.Headers.Add("Authorization", "Bearer " + token);
            }
            await _next(httpContext);
        }
    }
}
