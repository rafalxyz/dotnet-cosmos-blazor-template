namespace MyCompany.NewProject.Core.Results.Errors;

public sealed class UnauthorizedError : Error
{
    public UnauthorizedError(string code, string message)
        : base(code, message)
    {
    }

    public UnauthorizedError(string message)
        : base("Error.Unauthorized", message)
    {
    }
}
