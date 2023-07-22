using MyCompany.NewProject.Application.Abstractions.Messaging;
using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.Core.Results.Errors;
using MyCompany.NewProject.WebUi.Core.Notifications;
using MediatR;

namespace MyCompany.NewProject.WebUi.Core.Messaging;

public interface IDispatcher
{
    Task<Result> Send(ICommand request, CancellationToken cancellationToken = default);
    Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default);
    Task<Result<TResponse>> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default);
}

/// <remarks>
/// Based on <see href="https://github.com/jbogard/MediatR/issues/607"/>
/// </remarks>
internal sealed class Dispatcher : IDispatcher
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<Dispatcher> _logger;
    private readonly ISnackbarService _snackbarService;

    public Dispatcher(IServiceScopeFactory serviceScopeFactory, ILogger<Dispatcher> logger, ISnackbarService snackbarService)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _snackbarService = snackbarService;
    }

    public Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default)
    {
        return SendWithCustomServiceScope(request, cancellationToken);
    }

    public Task<Result> Send(ICommand request, CancellationToken cancellationToken = default)
    {
        return SendWithCustomServiceScope(request, cancellationToken);
    }

    public Task<Result<TResponse>> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default)
    {
        return SendWithCustomServiceScope(request, cancellationToken);
    }

    private async Task<TResponse> SendWithCustomServiceScope<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
        where TResponse : Result
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var result = await SendWithExceptionMapping(sender, request, cancellationToken);

        if (result is { IsFailure: true, Error: not NotFoundError })
        {
            _snackbarService.ShowError(result.Error.Message);
        }

        return result;
    }

    private async Task<TResponse> SendWithExceptionMapping<TResponse>(ISender sender, IRequest<TResponse> request, CancellationToken cancellationToken)
        where TResponse : Result
    {
        try
        {
            return await sender.Send(request, cancellationToken);
        }
        catch (Exception ex)
        {
            var error = new Error("UnhandledException", "An error has occurred during processing your request.");
            _logger.LogError(ex, "Unhandled exception has occurred.");

            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)Result.Failure(error);
            }

            var typedResult = typeof(Result)
                .GetMethods()
                .Single(x => x is { Name: nameof(Result.Failure), ContainsGenericParameters: true })
                .MakeGenericMethod(typeof(TResponse).GenericTypeArguments[0])
                .Invoke(null, new object?[] { error });

            return (TResponse)typedResult!;
        }
    }
}
