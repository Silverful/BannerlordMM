using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BL.API.Services.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogInformationSerialized<T>(this ILogger logger, string message, T obj)
        {
            logger.LogInformation(message + " " + JsonSerializer.Serialize(obj, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }));
        }
    }
}
