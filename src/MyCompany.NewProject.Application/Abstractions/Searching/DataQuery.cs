using MyCompany.NewProject.Application.Abstractions.Messaging;

namespace MyCompany.NewProject.Application.Abstractions.Searching;

public class DataQuery<TData> : IQuery<DataResponse<TData>>
{
    public int Skip { get; set; }
    public int Limit { get; set; } = 25;
    public string OrderBy { get; set; } = default!;
    public OrderDirection OrderDirection { get; set; } = OrderDirection.Ascending;
}
