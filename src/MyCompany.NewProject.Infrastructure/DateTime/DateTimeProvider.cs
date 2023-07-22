using MyCompany.NewProject.Core.Abstractions.DateTime;

namespace MyCompany.NewProject.Infrastructure.DateTime;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}
