using MyCompany.NewProject.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MyCompany.NewProject.Application.Abstractions.Searching;

public static class QueryableExtensions
{
    public static async Task<DataResponse<T>> ToDataResponse<T>(this IQueryable<T> source, DataQuery<T> query, CancellationToken cancellationToken)
    {
        var items = await source.Skip(query.Skip).Take(query.Limit).ToListAsync(cancellationToken);
        var totalItems = await source.CountAsync(cancellationToken);

        return new DataResponse<T>
        {
            Page = query.Skip,
            Size = query.Limit,
            TotalItems = totalItems,
            Items = items,
        };
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, DataQuery<T> query)
    {
        var orderByMethod = query.OrderDirection == OrderDirection.Ascending ? "OrderBy" : "OrderByDescending";

        ParameterExpression parameterExpression = Expression.Parameter(source.ElementType);
        MemberExpression memberExpression = Expression.Property(parameterExpression, query.OrderBy);

        MethodCallExpression orderByCall = Expression.Call(
            typeof(Queryable),
            orderByMethod,
            new Type[] { source.ElementType, memberExpression.Type },
            source.Expression,
            Expression.Quote(Expression.Lambda(memberExpression, parameterExpression)));

        return (IQueryable<T>)source.Provider.CreateQuery(orderByCall);
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, string? queryParam, Expression<Func<T, bool>> predicate)
    {
        return !queryParam.IsNullOrWhiteSpace() ? source.Where(predicate) : source;
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, DateTime? queryParam, Expression<Func<T, bool>> predicate)
    {
        return queryParam is not null ? source.Where(predicate) : source;
    }
}
