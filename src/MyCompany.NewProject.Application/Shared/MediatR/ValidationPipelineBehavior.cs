using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.Core.Results.Errors;
using FluentValidation;
using MediatR;

namespace MyCompany.NewProject.Application.Shared.MediatR;

internal sealed class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(
        IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var errors = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validator => validator.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage))
            .Distinct()
            .ToArray();

        if (errors.Length > 0)
        {
            return CreateValidationResult<TResponse>(errors);
        }

        return await next();
    }

    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        var aggregateValidationError = new AggregateValidationError("Validation errors occurred.", errors);

        if (typeof(TResult) == typeof(Result))
        {
            return (TResult)Result.Failure(aggregateValidationError);
        }

        return (TResult)typeof(Result)
            .GetMethods()
            .Single(x => x.Name == nameof(Result.Failure) && x.ContainsGenericParameters)!
            .MakeGenericMethod(typeof(TResult).GenericTypeArguments[0])
            .Invoke(null, new object?[] { aggregateValidationError })!;
    }
}
