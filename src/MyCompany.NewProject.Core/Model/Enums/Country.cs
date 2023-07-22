using Ardalis.SmartEnum;

namespace MyCompany.NewProject.Core.Model.Enums;

public sealed class Country : SmartEnum<Country, string>
{
    public static readonly Country Poland = new("6723fe22-f4e6-4a8f-ad69-694f1ab1f462", "Poland");
    public static readonly Country Uk = new("4825ab07-f5eb-4e35-88b1-17d60c702202", "United Kingdom");
    public static readonly Country Usa = new("34d7db73-3ebb-47d2-b077-1d841bf9c46d", "USA");

    public string Id => Value;

    private Country(string id, string name) : base(name: name, value: id)
    {
    }
}
