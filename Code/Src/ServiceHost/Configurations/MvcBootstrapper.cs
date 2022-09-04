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

        AddAriusMvc(services);
    }

    static void AddAriusMvc(IServiceCollection collection)
    {
        collection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        collection.AddControllers();
    }
}