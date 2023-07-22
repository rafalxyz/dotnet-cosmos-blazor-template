namespace MyCompany.NewProject.Core.Results.Errors;

public sealed class NotFoundError : Error
{
    public NotFoundError(string code, string message)
        : base(code, message)
    {
    }

    public NotFoundError(string message)
        : base("Error.NotFound", message)
    {
    }
}
