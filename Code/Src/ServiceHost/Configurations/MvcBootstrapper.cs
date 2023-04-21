using Microsoft.Extensions.DependencyInjection.Extensions;
using Neo.Infrastructure.Framework.Configurations;

namespace ServiceHost.Configurations;

public class MvcBootstrapper : IBootstrapper
{
    public void Bootstrap(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        AddNeoMvc(services);
    }

    static void AddNeoMvc(IServiceCollection services)
    {
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddControllers()
            .AddMvcOptions(x =>
            {
                x.EnableEndpointRouting = false;
            });
    }
}