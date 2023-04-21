using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Neo.Infrastructure.Framework.Configurations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Neo.Infrastructure.Framework.Swagger;

public class SwaggerBootstrapper : IBootstrapper
{
    private readonly IConfiguration _configuration;

    public SwaggerBootstrapper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Bootstrap(IServiceCollection services)
    {
        var options = _configuration.GetSection("swagger").Get<SwaggerOptions>();
        if (!options.Enabled)
            return;

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(options.Name, new OpenApiInfo { Title = options.Title, Version = options.Version });
            c.TagActionsBy(a =>
            {
                var controllerName = a.ActionDescriptor.RouteValues["controller"].Replace("Query", "");
                return new List<string>() { controllerName };
            });
            c.CustomSchemaIds(i => i.FullName);
            if (options.IncludeSecurity)
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.OperationFilter<BasicAuthOperationsFilter>();
            }
        });

        services.Remove<ISwaggerProvider>();
        services.AddTransient<ISwaggerProvider, SwaggerGenerator>();
    }
}