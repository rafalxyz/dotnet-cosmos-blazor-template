using MyCompany.NewProject.AzureFunctions.Shared;
using MyCompany.NewProject.AzureFunctions.Shared.ChangeFeed;
using MyCompany.NewProject.Core.Model.Dictionaries;
using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;

namespace MyCompany.NewProject.AzureFunctions.Dictionaries;

public sealed class DictionariesChangeFeedProcessor
{
    private readonly IEntityChangeFeedProcessor _entityChangeFeedProcessor;

    public DictionariesChangeFeedProcessor(IEntityChangeFeedProcessor entityChangeFeedProcessor)
    {
        _entityChangeFeedProcessor = entityChangeFeedProcessor;
    }

    [Function("DictionariesChangeFeedProcessor")]
    public async Task Run([CosmosDBTrigger(
        databaseName: Constants.Database.DatabaseName,
        containerName: Constants.Database.DictionaryContainerName,
        Connection = Constants.Database.ConnectionString,
        LeaseContainerName = Constants.Database.ChangeFeedLeaseContainerName,
        CreateLeaseContainerIfNotExists = true)]string dictionaries)
    {
        await _entityChangeFeedProcessor.Process<Dictionary>(dictionaries);
    }
}
