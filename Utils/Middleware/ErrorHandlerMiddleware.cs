using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Middlewares;
using System.Net;
using System.Text.Json;

namespace Utils.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogRequestMiddleware> _logger;
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<LogRequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                if (error.InnerException is KeyNotFoundException)
                {
                    error = error.InnerException as KeyNotFoundException;
                }

                if (error.InnerException is BadRequestException)
                {
                    error = error.InnerException as BadRequestException;
                }

                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = error.Message;
                switch (error)
                {
                    case BadRequestException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                //Este loggeo es para guardar la IP, al estar en un entorno de dev la respuesta no es correcta.
                //_logger.LogError($"Sender IP: {context.Connection.RemoteIpAddress}");
                _logger.LogError(error, $"Error: {error.Message} - {response.StatusCode}");
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}