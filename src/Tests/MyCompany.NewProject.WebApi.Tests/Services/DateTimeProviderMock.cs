using MyCompany.NewProject.Core.Abstractions.DateTime;

namespace MyCompany.NewProject.WebApi.Tests.Services;

internal class DateTimeProviderMock : IDateTimeProvider
{
    private static DateTimeOffset? _now;

    public static void SetNow(DateTimeOffset now) => _now = now;

    public static DateTimeOffset GetNow() => _now ?? DateTimeOffset.UtcNow;

    public static void Clear() => _now = null;

    public DateTimeOffset Now => _now ?? DateTimeOffset.UtcNow;
}
