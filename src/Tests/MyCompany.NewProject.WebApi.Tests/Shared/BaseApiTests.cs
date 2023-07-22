using MyCompany.NewProject.Persistence;
using MyCompany.NewProject.WebApi.Tests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MyCompany.NewProject.WebApi.Tests.Shared;

[Collection(nameof(SharedTestCollection))]
public abstract class BaseApiTests
{
    private readonly NewProjectFactory _factory;

    protected BaseApiTests(NewProjectFactory factory)
    {
        _factory = factory;

        DateTimeProviderMock.Clear();
        // ServiceBusSenderMock.Clear();
        FailureRepositoryMock.Clear();
    }

    protected async Task WithServiceScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = _factory.Services.CreateScope();
        await action(scope.ServiceProvider);
    }

    protected async Task WithDatabaseAsync(Func<ApplicationDbContext, Task> action)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await action(context);
    }
}
