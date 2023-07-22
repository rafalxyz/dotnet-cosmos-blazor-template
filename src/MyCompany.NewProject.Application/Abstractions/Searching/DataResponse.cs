namespace MyCompany.NewProject.Application.Abstractions.Searching;

public class DataResponse<TData>
{
    public required int Page { get; init; }
    public required int Size { get; init; }
    public required int TotalItems { get; init; }
    public required ICollection<TData> Items { get; init; }
}
