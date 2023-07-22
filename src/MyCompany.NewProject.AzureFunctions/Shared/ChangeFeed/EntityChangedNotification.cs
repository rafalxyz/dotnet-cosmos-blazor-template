using MyCompany.NewProject.Core.Model;
using MediatR;

namespace MyCompany.NewProject.AzureFunctions.Shared.ChangeFeed;

public sealed record EntityChangedNotification<TEntity>(TEntity Data) : INotification
    where TEntity : Entity;
