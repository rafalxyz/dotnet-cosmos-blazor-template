namespace MyCompany.NewProject.Core.Model.Dictionaries;

public abstract class Dictionary : Entity, IDeletable, IHasKey
{
    public bool Deleted { get; set; }

    /// <summary>
    /// Unique key that should be set by Entity Framework automatically
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// Returns unique key of the dictionary.
    /// </summary>
    public abstract string CalculateKey();
}