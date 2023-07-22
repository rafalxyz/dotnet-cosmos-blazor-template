using MyCompany.NewProject.AzureFunctions.Shared.Persistence;
using MyCompany.NewProject.Core.Results;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyCompany.NewProject.AzureFunctions.Shared.Failures;

public interface IFailureRepository
{
    Task Save(JsonElement jsonElement, Error error);
}

internal sealed class FailureRepository : IFailureRepository
{
    private readonly FunctionsDbContext _db;

    public FailureRepository(FunctionsDbContext db)
    {
        _db = db;
    }

    public async Task Save(JsonElement jsonElement, Error error)
    {
        var notificationFailure = Failure.For(jsonElement, error);
        _db.Failures.Add(notificationFailure);
        await _db.SaveChangesAsync();
    }
}