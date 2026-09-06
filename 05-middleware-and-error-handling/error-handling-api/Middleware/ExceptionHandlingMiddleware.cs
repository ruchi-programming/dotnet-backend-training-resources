using ErrorHandlingApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ErrorHandlingApi.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionHandlingMiddleware>
        _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
            {
                throw;
            }

            await WriteProblemDetailsAsync(
                context,
                exception);
        }
    }

    private async Task WriteProblemDetailsAsync(
        HttpContext context,
        Exception exception)
    {
        int statusCode;
        string title;
        string detail;

        switch (exception)
        {
            case ResourceNotFoundException:
                statusCode =
                    StatusCodes.Status404NotFound;
                title = "Resource not found";
                detail = exception.Message;

                _logger.LogWarning(
                    exception,
                    "A requested resource was not found. " +
                    "Trace identifier: {TraceIdentifier}",
                    context.TraceIdentifier);
                break;

            case ArgumentException:
                statusCode =
                    StatusCodes.Status400BadRequest;
                title = "Invalid request";
                detail = exception.Message;

                _logger.LogWarning(
                    exception,
                    "Request validation failed. " +
                    "Trace identifier: {TraceIdentifier}",
                    context.TraceIdentifier);
                break;

            default:
                statusCode =
                    StatusCodes.Status500InternalServerError;
                title = "An unexpected error occurred";
                detail =
                    "The server could not complete the request.";

                _logger.LogError(
                    exception,
                    "An unhandled exception occurred. " +
                    "Trace identifier: {TraceIdentifier}",
                    context.TraceIdentifier);
                break;
        }

        ProblemDetails problem = new()
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] =
            context.TraceIdentifier;

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType =
            "application/problem+json";

        await context.Response.WriteAsJsonAsync(
            problem,
            cancellationToken: context.RequestAborted);
    }
}
