using System.Net;
using System.Text.Json;
using Scheduler.Constants;
using Scheduler.Exceptions;

namespace Scheduler.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
        catch (BaseException baseException)
        {
            await HandleExceptionAsync(context, baseException);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            EntityNotFoundException e => (HttpStatusCode.NotFound, e.GetErrorData()),
            EntityAlreadyExistExceptions e => (HttpStatusCode.Conflict, e.GetErrorData()),
            BadRequestException e =>  (HttpStatusCode.BadRequest, e.GetErrorData()),
            FileNotFoundException e => (HttpStatusCode.NotFound, ExceptionsCode.FileNotFound),
            _ => (HttpStatusCode.InternalServerError, null)
        };

        _logger.LogError(exception, "Произошла ошибка: {Message}", message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            StatusCode = statusCode,
            Message = message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}