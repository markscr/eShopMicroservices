using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public class eShopExceptionHandler(ILogger<eShopExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        logger.LogError(
            "Error message: {exceptionMessage}, Time of occurrence {time}.",
            exception.Message,
            DateTime.UtcNow
        );

        int statusCode = exception switch
        {
            InternalServerException => (
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
            ),
            BadRequestException => (
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            ValidationException => (
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            NotFoundException => (httpContext.Response.StatusCode = StatusCodes.Status404NotFound),
            _ => (httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError),
        };

        ProblemDetails problemDetails = new()
        {
            Title = exception.GetType().Name,
            Detail = exception.Message,
            Status = statusCode,
            Instance = httpContext.Request.Path,
        };

        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
