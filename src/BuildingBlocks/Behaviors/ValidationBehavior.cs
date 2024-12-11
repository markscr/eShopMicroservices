using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;

/// <summary>
/// ValidationBehavior class that uses the MediatR pipeline to validate incoming requests before calling the handlers.
/// </summary>
/// <typeparam name="TRequest">Request object</typeparam>
/// <typeparam name="TResponse">Response object</typeparam>
/// <param name="validators"></param>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse> // Applies for command requests only, not queries
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var validationContext = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            validators.Select(validator =>
                validator.ValidateAsync(validationContext, cancellationToken)
            )
        );
        var failures = validationResults
            .Where(validator => validator.Errors.Any())
            .SelectMany(v => v.Errors)
            .ToList();

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
