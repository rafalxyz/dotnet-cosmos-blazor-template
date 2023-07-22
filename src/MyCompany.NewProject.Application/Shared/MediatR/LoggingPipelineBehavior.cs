using MyCompany.NewProject.Core.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyCompany.NewProject.Application.Shared.MediatR;

internal sealed class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (response is Result { IsFailure: true } result)
        {
            var jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            _logger.LogWarning("Error during request processing: Request: {Request}, Error: {Error}.",
                    JsonSerializer.Serialize(request, request.GetType(), jsonSerializerOptions),
                    JsonSerializer.Serialize(result.Error, result.Error.GetType(), jsonSerializerOptions));
        }

        return response;
    }
}
