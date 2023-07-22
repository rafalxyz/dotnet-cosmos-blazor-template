namespace MyCompany.NewProject.Core.Results.Errors;

public sealed class ExceptionError : Error
{
    public Exception Exception { get; }

    public ExceptionError(Exception exception)
        : base("Error.Unknown", exception.Message)
    {
        Exception = exception;
    }
}
