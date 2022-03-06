using BL.API.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BL.API.WebHost.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                using StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8);
                var body = await reader.ReadToEndAsync();

                var message = $"{httpContext.Request.Path.Value}/{body}";
                _logger.LogError(ex, message);

                var response = httpContext.Response;
                response.ContentType = "application/json";

                switch (ex)
                {
                    case AlreadyExistsException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case GuidCantBeParsedException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case NotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case NotAuthorized:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = ex?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
