namespace MyCompany.NewProject.Core.Model.Dictionaries;

public sealed class Location : Dictionary
{
    public override string CalculateKey() => Name;

    public string Name { get; set; } = null!;

    public LocationCountry Country { get; set; } = null!;

    public sealed record LocationCountry(string Id, string Name);
}
