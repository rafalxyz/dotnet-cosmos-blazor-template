using MyCompany.NewProject.Core.Model;
using MyCompany.NewProject.DatabaseSeeder.Seeders;
using MyCompany.NewProject.Persistence;

namespace MyCompany.NewProject.DatabaseSeeder;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;

    public DatabaseSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task RunAsync()
    {
        await RemoveExistingDataAsync();
        await SeedDataAsync();
    }

    private async Task SeedDataAsync()
    {
        var users = UsersSeeder.GetUsers();
        _context.AddRange(users);

        _context.ChangeTracker
            .Entries()
            .Select(x => x.Entity)
            .OfType<IHasKey>()
            .ToList()
            .ForEach(x => x.Key = x.CalculateKey());

        Console.WriteLine("Seeding data...");
        await _context.SaveChangesAsync();
        Console.WriteLine("Data seeded.");
    }

    private async Task RemoveExistingDataAsync()
    {
        Console.WriteLine("Removing existing data...");

        _context.RemoveRange(_context.Users.ToList());
        await _context.SaveChangesAsync();

        Console.WriteLine("Existing data removed.");
    }
}
