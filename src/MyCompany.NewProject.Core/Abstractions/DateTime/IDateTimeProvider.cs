namespace MyCompany.NewProject.Core.Abstractions.DateTime;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}
