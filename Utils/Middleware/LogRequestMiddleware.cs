using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Middlewares
{
    public class LogRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogRequestMiddleware> _logger;

        public LogRequestMiddleware(RequestDelegate next, ILogger<LogRequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var builder = new StringBuilder();
            builder.Append("Request: ");
            builder.Append(context.Request.Method);
            builder.Append(" - ");
            builder.Append(context.Request.Path);

            //Este loggeo es para guardar la IP, al estar en un entorno de dev la respuesta no es correcta.
            //_logger.LogInformation($"Sender IP: {context.Connection.RemoteIpAddress}");
            _logger.LogInformation(builder.ToString());

            await _next(context);
        }
    }
}