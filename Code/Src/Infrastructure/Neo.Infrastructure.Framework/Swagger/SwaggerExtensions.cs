using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Neo.Infrastructure.Framework.Configurations;

namespace Neo.Infrastructure.Framework.Swagger;

public static class SwaggerExtensions
{
    public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder builder)
    {
        var options = builder.ApplicationServices.GetService<IConfiguration>()
            .GetOptions<SwaggerOptions>("swagger");
        if (!options.Enabled)
        {
            return builder;
        }

        var routePrefix = string.IsNullOrWhiteSpace(options.RoutePrefix)
            ? "swagger"
            : options.RoutePrefix;

        builder.UseStaticFiles()
            .UseSwagger(c =>
            {
                c.RouteTemplate = routePrefix + "/{documentName}/swagger.json";
                if (string.IsNullOrWhiteSpace(options.BasePath))
                    return;
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    OpenApiPaths paths = new OpenApiPaths();
                    foreach (var (key, value) in swaggerDoc.Paths)
                    {
                        paths.Add(key.Replace("/api", options.BasePath), value);
                    }

                    swaggerDoc.Paths = paths;
                });
            });

        return builder.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"/{routePrefix}/{options.Name}/swagger.json", options.Title);
            c.RoutePrefix = routePrefix;
        });
    }
}