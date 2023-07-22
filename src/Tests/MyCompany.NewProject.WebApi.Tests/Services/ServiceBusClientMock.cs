// using Azure.Messaging.ServiceBus;
//
// namespace MyCompany.NewProject.WebApi.Tests.Services;
//
// internal class ServiceBusClientMock : ServiceBusClient
// {
//     public override ServiceBusSender CreateSender(string queueOrTopicName)
//     {
//         return new ServiceBusSenderMock();
//     }
// }
//
// internal class ServiceBusSenderMock : ServiceBusSender
// {
//     private static readonly List<ServiceBusMessage> _messages = new();
//     public static IReadOnlyCollection<ServiceBusMessage> Messages => _messages.AsReadOnly();
//
//     public static void Clear()
//     {
//         _messages.Clear();
//     }
//
//     public override Task SendMessageAsync(ServiceBusMessage message, CancellationToken cancellationToken = default)
//     {
//         _messages.Add(message);
//         return Task.CompletedTask;
//     }
// }