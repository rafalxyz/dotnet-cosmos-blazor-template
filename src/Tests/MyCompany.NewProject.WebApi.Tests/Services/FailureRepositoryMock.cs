using MyCompany.NewProject.AzureFunctions.Shared.Failures;
using MyCompany.NewProject.Core.Results;
using System.Text.Json;

namespace MyCompany.NewProject.WebApi.Tests.Services;

internal class FailureRepositoryMock : IFailureRepository
{
    private static readonly List<Error> _errors = new();
    public static IReadOnlyCollection<Error> Errors => _errors.AsReadOnly();

    public Task Save(JsonElement jsonElement, Error error)
    {
        _errors.Add(error);
        return Task.CompletedTask;
    }

    public static void Clear()
    {
        _errors.Clear();
    }
}
