namespace MyCompany.NewProject.Core.Model;

public interface IHasKey
{
    string Key { get; set; }
    string CalculateKey();
}