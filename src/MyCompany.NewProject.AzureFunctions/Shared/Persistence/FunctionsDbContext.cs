using MyCompany.NewProject.AzureFunctions.Shared.Failures;
using Microsoft.EntityFrameworkCore;

namespace MyCompany.NewProject.AzureFunctions.Shared.Persistence;

public class FunctionsDbContext : DbContext
{
    public DbSet<Failure> Failures => Set<Failure>();

    public FunctionsDbContext(DbContextOptions<FunctionsDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Failure>().ToContainer(nameof(Failure)).HasPartitionKey(x => x.Id);
    }
}
