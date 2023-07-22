using MyCompany.NewProject.Core.Model.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCompany.NewProject.Persistence.Configuration;

internal static class EntityTypeBuilderExtensions
{
    public static void ConfigureDictionary<TDictionary>(this EntityTypeBuilder<TDictionary> builder)
        where TDictionary : Dictionary
    {
        builder.HasQueryFilter(x => !x.Deleted);
        builder.Property(x => x.EntityTag).IsETagConcurrency();
        builder.ToContainer(nameof(Dictionary)).HasPartitionKey("Discriminator");
    }
}

internal sealed class DictionaryConfiguration :
    IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ConfigureDictionary();
    }
}