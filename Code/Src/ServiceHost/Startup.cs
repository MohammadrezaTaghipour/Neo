using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Infrastructure.EventStore.Configurations;
using Neo.Infrastructure.Framework.Configurations;
using Neo.Infrastructure.Framework.Swagger;
using Neo.Infrastructure.Persistence.ES;
using ServiceHost.Configurations;
using ServiceHost.Handlers;

namespace ServiceHost;

public class Startup
{
    public readonly IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        BootstrapBuilder
            .GetInstance(services)
            .With(new CoreBootstrapper())
            .With(new NeoBootstrapper())
            .With(new EsDbBootstrapper(Configuration,
                typeof(StreamEventTypeDefined).Assembly))
            .With(new EsDbSubscriptionBootstrapper(Configuration,
                typeof(TestEventHandler).Assembly))
            .With(new SwaggerBootstrapper(Configuration))
            .With(new MvcBootstrapper())
            .Build();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwaggerDocs();
        app.UseCors("CorsPolicy");
        app.UseMvcWithDefaultRoute();
    }
}