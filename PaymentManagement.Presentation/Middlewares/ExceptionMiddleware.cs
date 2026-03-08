using System.Text.Json;
using FluentValidation;
using PaymentManagement.Domain;

namespace PaymentManagement.Presentation.Middlewares
{
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message, errors) = ex switch
            {
                BusinessException businessEx => (StatusCodes.Status400BadRequest, businessEx.Message, (object?)null),
                NotFoundException => (StatusCodes.Status404NotFound, ex.Message, null),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, ex.Message, null),
                ValidationException validationEx => (StatusCodes.Status400BadRequest, "Validation failed", validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })),
                ArgumentException => (StatusCodes.Status400BadRequest, ex.Message, null),
                InvalidOperationException => (StatusCodes.Status400BadRequest, ex.Message, null),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", (object?)null)
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                success = false,
                message,
                errors,
                timestamp = DateTime.UtcNow
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }
}
