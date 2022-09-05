using Neo.Infrastructure.EventStore.Configs;
using Neo.Infrastructure.Framework.Configurations;
using Neo.Infrastructure.Framework.Swagger;
using Neo.Infrastructure.Persistence.ES;
using ServiceHost.Configurations;

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
            .With(new NeoBootstrapper())
            .With(new EsDbBootstrapper(Configuration,
                typeof(StreamEventTypeRepository).Assembly))
            .With(new SwaggerBootstrapper(Configuration))
            .With(new MvcBootstrapper())
            .Build();
    }
}