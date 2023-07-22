namespace MyCompany.NewProject.Core.Results.Errors;

public sealed class ValidationError : Error
{
    public ValidationError(string code, string message)
        : base(code, message)
    {
    }

    public ValidationError(string message)
        : base("Error.NotValid", message)
    {
    }
}
