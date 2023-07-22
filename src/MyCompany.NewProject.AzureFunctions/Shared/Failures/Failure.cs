using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.Core.Results.Errors;
using System;
using System.Text.Json;

namespace MyCompany.NewProject.AzureFunctions.Shared.Failures;

public sealed class Failure
{
    public required string Id { get; init; }
    public required string Data { get; init; }
    public required string ErrorMessage { get; init; }
    public string? ExceptionStackTrace { get; init; }

    public static Failure For(JsonElement jsonElement, Error error)
    {
        return new Failure
        {
            Id = Guid.NewGuid().ToString(),
            Data = jsonElement.ToString(),
            ErrorMessage = error.Message,
            ExceptionStackTrace = (error as ExceptionError)?.Exception.StackTrace
        };
    }
}