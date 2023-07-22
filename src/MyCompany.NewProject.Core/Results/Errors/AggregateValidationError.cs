namespace MyCompany.NewProject.Core.Results.Errors;

public sealed class AggregateValidationError : Error
{
    public static AggregateValidationError FromFailedResults(IEnumerable<Result> failedResults)
    {
        if (failedResults.Any(x => x.IsSuccess))
        {
            throw new InvalidOperationException("Cannot create aggregate validation error from successful result.");
        }

        return new AggregateValidationError("Validation errors occurred.", failedResults.Select(x => x.Error).ToList());
    }

    public IReadOnlyList<Error> Errors { get; }

    public AggregateValidationError(string message, IReadOnlyList<Error> errors)
        : this(nameof(AggregateValidationError), message, errors)
    {
    }

    public AggregateValidationError(string code, string message, IReadOnlyList<Error> errors)
        : base(code, message)
    {
        Errors = errors.SelectMany(error => error is AggregateValidationError aggregateValidationError
            ? aggregateValidationError.Errors
            : new[] { error })
            .ToList();
    }
}