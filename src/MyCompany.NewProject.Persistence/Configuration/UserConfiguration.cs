using MyCompany.NewProject.Core.Model.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCompany.NewProject.Persistence.Configuration;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasQueryFilter(x => !x.Deleted);
        builder.Property(x => x.EntityTag).IsETagConcurrency();
        builder.ToContainer(nameof(User)).HasPartitionKey(x => x.Email);
    }
}
