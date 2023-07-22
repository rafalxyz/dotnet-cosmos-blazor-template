using MyCompany.NewProject.AzureFunctions.App;
using MyCompany.NewProject.AzureFunctions.Shared.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((host, services) =>
    {
        services.AddFunctionsServices(host.Configuration);
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var functionsDbContext = scope.ServiceProvider.GetRequiredService<FunctionsDbContext>();
    functionsDbContext.Database.EnsureCreated();
}

await host.RunAsync();
