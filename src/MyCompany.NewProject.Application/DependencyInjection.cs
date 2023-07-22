using MyCompany.NewProject.Application.Features.Dictionaries;
using MyCompany.NewProject.Application.Shared.MediatR;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MyCompany.NewProject.Application;

public static class DependencyInjection
{
    private static Assembly Assembly => typeof(DependencyInjection).Assembly;

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddValidatorsFromAssembly(Assembly, includeInternalTypes: true);
        services.AddAutoMapper(Assembly);

        services.AddTransient<IDictionaryService, DictionaryService>();

        return services;
    }
}
