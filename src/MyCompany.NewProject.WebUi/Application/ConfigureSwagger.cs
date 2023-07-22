using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace MyCompany.NewProject.WebUi.Application;

public static class ConfigureSwagger
{
    public static void AddAppSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.DocumentFilter<PathPrefixDocumentFilter>();

            c.SwaggerDoc("v1", new OpenApiInfo { Title = "New Project API", Version = "v1" });

            c.SupportNonNullableReferenceTypes();

            c.TagActionsBy(api =>
            {
                if (api.GroupName != null)
                {
                    return new[] { api.GroupName };
                }

                if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    return new[] { controllerActionDescriptor.ControllerName };
                }

                throw new InvalidOperationException("Unable to determine tag for endpoint.");
            });

            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                    },
                    Array.Empty<string>()
                }
            });
            c.DocInclusionPredicate((name, api) => true);
        });
    }

    public static void UseAppSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            {
                swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/api" } };
            });
        });
        app.UseSwaggerUI(c =>
        {
            c.DocExpansion(DocExpansion.None);
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "New Project API V1");
        });
    }
}

public class PathPrefixDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var editedPaths = new OpenApiPaths();
        foreach (var (key, value) in swaggerDoc.Paths)
        {
            var newKey = key.Replace("api/", string.Empty);
            editedPaths.Add(newKey, value);
        }
        swaggerDoc.Paths = editedPaths;
    }
}