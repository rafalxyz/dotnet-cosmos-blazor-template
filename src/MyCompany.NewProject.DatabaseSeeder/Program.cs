using MyCompany.NewProject.DatabaseSeeder;
using MyCompany.NewProject.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets(typeof(DatabaseSeeder).Assembly)
    .Build();

var context = new ApplicationDbContext(
    new DbContextOptionsBuilder<ApplicationDbContext>().UseCosmos(
        configuration["Cosmos:ConnectionString"]!,
        configuration["Cosmos:DatabaseName"]!).Options);

var seeder = new DatabaseSeeder(context);

await seeder.RunAsync();