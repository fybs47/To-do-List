using System.Net;
using System.Text.Json;
using Application.Exceptions;

namespace To_do_List.Middleware;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionHandler(
        RequestDelegate next, 
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "An unexpected error occurred";
        object? details = null;

        switch (exception)
        {
            case NotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                message = exception.Message;
                break;
            case ForbiddenException:
                statusCode = (int)HttpStatusCode.Forbidden;
                message = exception.Message;
                break;
            case UnauthorizedException:
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = exception.Message;
                break;
            case ValidationException validationEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = "Validation failed";
                details = validationEx.Errors;
                break;
            case ConflictException:
                statusCode = (int)HttpStatusCode.Conflict;
                message = exception.Message;
                break;
        }

        context.Response.StatusCode = statusCode;

        var response = new
        {
            StatusCode = statusCode,
            Message = message,
            Details = details,
            StackTrace = _env.IsDevelopment() ? exception.StackTrace : null
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}