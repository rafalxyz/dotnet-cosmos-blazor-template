using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.Core.Results.Errors;
using MyCompany.NewProject.WebApi.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace MyCompany.NewProject.WebApi.Controllers.Base;

[ApiController]
[Authorize(Policy = Policies.ApiAccess)]
public abstract class ApiControllerBase : ControllerBase
{
    protected ISender Sender => HttpContext.RequestServices.GetRequiredService<ISender>();

    protected IActionResult HandleResult<T>(Result<T> result, Func<T, IActionResult> onSuccess)
    {
        if (result.IsSuccess)
        {
            return onSuccess(result.Value);
        }

        return MapToActionResult(result);
    }

    protected IActionResult HandleResult(Result result, Func<IActionResult> onSuccess)
    {
        if (result.IsSuccess)
        {
            return onSuccess();
        }
        return MapToActionResult(result);
    }

    private IActionResult MapToActionResult(Result result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            { Error: AggregateValidationError aggregateValidationError } => BadRequest(CreateProblemDetails("Bad Request", StatusCodes.Status400BadRequest, aggregateValidationError, aggregateValidationError.Errors)),
            { Error: ValidationError } => Conflict(CreateProblemDetails("Conflict", StatusCodes.Status409Conflict, result.Error, new Error[] { result.Error })),
            { Error: NotFoundError } => NotFound(CreateProblemDetails("Not Found", StatusCodes.Status404NotFound, result.Error)),
            { Error: UnauthorizedError } => Unauthorized(CreateProblemDetails("Unauthorized", StatusCodes.Status401Unauthorized, result.Error)),
            _ => BadRequest(CreateProblemDetails("Bad Request", StatusCodes.Status400BadRequest, result.Error))
        };
    }

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        IReadOnlyList<Error>? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } },
        };
}
