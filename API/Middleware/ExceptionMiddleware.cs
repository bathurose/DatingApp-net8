using API.Errors;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, ex.Message); // print error to terminal
                
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = env.IsDevelopment() ? new ApiException(httpContext.Response.StatusCode, ex.Message, ex.StackTrace)
                    : new ApiException(httpContext.Response.StatusCode, ex.Message, "Internal server error");

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                var json = JsonSerializer.Serialize(response, options);

                await httpContext.Response.WriteAsync(json);
            }
        }
    }
}
