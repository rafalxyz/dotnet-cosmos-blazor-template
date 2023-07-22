using MyCompany.NewProject.AzureFunctions.Shared.Failures;
using MyCompany.NewProject.Core.Model;
using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.Core.Results.Errors;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyCompany.NewProject.AzureFunctions.Shared.ChangeFeed;

public interface IEntityChangeFeedProcessor
{
    Task Process<TBaseEntity>(string entities) where TBaseEntity : Entity;
}

internal sealed class EntityChangeFeedProcessor : IEntityChangeFeedProcessor
{
    private readonly ILogger<EntityChangeFeedProcessor> _logger;
    private readonly IPublisher _publisher;
    private readonly IFailureRepository _failureRepository;

    public EntityChangeFeedProcessor(
        ILogger<EntityChangeFeedProcessor> logger,
        IPublisher publisher,
        IFailureRepository failureRepository)
    {
        _logger = logger;
        _publisher = publisher;
        _failureRepository = failureRepository;
    }

    public async Task Process<TBaseEntity>(string entities)
        where TBaseEntity : Entity
    {
        using var jsonDocument = JsonDocument.Parse(entities);

        foreach (var jsonElement in jsonDocument.RootElement.EnumerateArray())
        {
            var result = await ProcessEntity<TBaseEntity>(jsonElement);
            if (result.IsFailure)
            {
                await _failureRepository.Save(jsonElement, result.Error);
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
    private async Task<Result> ProcessEntity<TBaseEntity>(JsonElement jsonElement)
        where TBaseEntity : Entity
    {
        var entityTypeResult = EntityTypeProvider.GetEntityType<TBaseEntity>(jsonElement);

        if (!entityTypeResult.IsSuccess)
        {
            _logger.LogError(entityTypeResult.Error.Message);
            return entityTypeResult.Error;
        }

        try
        {
            var entity = jsonElement.Deserialize(entityTypeResult.Value);
            var notification = new EntityChangedNotification<TBaseEntity>((TBaseEntity)entity!);
            await _publisher.Publish(notification);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during entity processing.");
            return new ExceptionError(ex);
        }

        return Result.Success();
    }
}