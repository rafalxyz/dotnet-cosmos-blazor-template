using MyCompany.NewProject.Core.Model;
using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.Core.Results.Errors;
using System;
using System.Linq;
using System.Text.Json;

namespace MyCompany.NewProject.AzureFunctions.Shared.ChangeFeed;

internal static class EntityTypeProvider
{
    private const string DiscriminatorPropertyName = "Discriminator";

    public static Result<Type> GetEntityType<TBaseEntity>(JsonElement jsonElement)
        where TBaseEntity : Entity
    {
        if (!jsonElement.TryGetProperty(DiscriminatorPropertyName, out var discriminatorElement))
        {
            return new ValidationError($"Discriminator does not exist for document with ID: {jsonElement.GetProperty("id")}");
        }

        var discriminator = discriminatorElement.ToString();

        var entityType = typeof(Entity).Assembly.GetTypes()
            .SingleOrDefault(x => x.Name == discriminator && x.IsAssignableTo(typeof(TBaseEntity)));

        if (entityType is null)
        {
            return new ValidationError($"Could not find entity type with name: {discriminator} that is assignable to {typeof(TBaseEntity).Name}.");
        }

        return entityType;
    }
}