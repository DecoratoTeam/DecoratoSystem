using Application.Dtos;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace DecorteeSystem.MiddleWare
{
    public class GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IWebHostEnvironment env,
        IConfiguration configuration)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;
        private readonly IWebHostEnvironment _env = env;
        private readonly IConfiguration _configuration = configuration;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");

                var includeDetails = _env.IsDevelopment() ||
                                     _configuration.GetValue("Diagnostics:IncludeExceptionDetails", defaultValue: false);

                await HandleExceptionAsync(context, ex, includeDetails);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, bool includeDetails)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var message = includeDetails
                ? exception.ToString()
                : "An unexpected error occurred. Please try again later.";

            var response = GeneralResponseDto<object>.Fail(
                ErrorType.ServerError,
                message);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                WriteIndented = true
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}