using MyCompany.NewProject.AzureFunctions.Shared.ChangeFeed;
using MyCompany.NewProject.Core.Model.Dictionaries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MyCompany.NewProject.AzureFunctions.Dictionaries;

internal sealed class SendNotificationsEventHandler : INotificationHandler<EntityChangedNotification<Dictionary>>
{
    public Task Handle(EntityChangedNotification<Dictionary> notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
