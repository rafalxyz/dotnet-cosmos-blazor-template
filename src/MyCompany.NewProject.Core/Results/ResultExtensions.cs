namespace MyCompany.NewProject.Core.Results;

public static class ResultExtensions
{
    public static T ValueOrDefault<T>(this Result<T> result, Func<T> defaultValueFactory)
    {
        return result.IsSuccess ? result.Value : defaultValueFactory();
    }

    public static async Task<T> ValueOrDefault<T>(this Task<Result<T>> resultTask, Func<T> defaultValueFactory)
    {
        var result = await resultTask;
        return result.ValueOrDefault(defaultValueFactory);
    }
}
