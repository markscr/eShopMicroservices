using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse> // Applies for all requests, commands and queries
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation(
            "[START] Handle Request={Request} - Response={Response} - RequestData={RequestData}.",
            typeof(TRequest).Name,
            typeof(TResponse).Name,
            request
        );

        Stopwatch timer = new();
        timer.Start();

        TResponse response = await next();

        timer.Stop();
        TimeSpan timeElapsed = timer.Elapsed;

        if (timeElapsed.Seconds > 3)
        {
            logger.LogWarning(
                "[PERFORMANCE] The Request {Request} took {timeElapsed}.",
                typeof(TRequest),
                timeElapsed.Seconds
            );
        }

        logger.LogInformation(
            "[END] Finished {Request} with Response {Response} in {timeElapsed} milliseconds.",
            typeof(TRequest),
            typeof(TResponse),
            timeElapsed.Milliseconds
        );

        return response;
    }
}
