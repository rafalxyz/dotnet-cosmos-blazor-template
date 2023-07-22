using MyCompany.NewProject.Application.Abstractions.Searching;
using MyCompany.NewProject.Core.Results;
using MudBlazor;

namespace MyCompany.NewProject.WebUi.Core.MudBlazor;

public static class MudGridExtensions
{
    public static void MapToQuery<TGridItem>(
        this GridState<TGridItem> gridState,
        DataQuery<TGridItem> query,
        string defaultOrderByProperty,
        OrderDirection orderDirection = OrderDirection.Ascending)
    {
        query.Skip = gridState.Page * gridState.PageSize;
        query.Limit = gridState.PageSize;

        if (gridState.SortDefinitions.Any())
        {
            var sortDefinition = gridState.SortDefinitions.First();
            query.OrderBy = sortDefinition.SortBy;
            query.OrderDirection = sortDefinition.Descending
                ? OrderDirection.Descending
                : OrderDirection.Ascending;
        }
        else
        {
            query.OrderBy = defaultOrderByProperty;
            query.OrderDirection = orderDirection;
        }
    }

    public static GridData<T> MapToGridData<T>(this Result<DataResponse<T>> result)
    {
        if (!result.IsSuccess)
        {
            return new GridData<T> { Items = new List<T>(), TotalItems = 0 };
        }

        return new GridData<T>
        {
            Items = result.Value.Items,
            TotalItems = result.Value.TotalItems
        };
    }
}
