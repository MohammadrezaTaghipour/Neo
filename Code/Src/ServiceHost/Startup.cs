using Neo.Application;
using Neo.Application.StreamEventTypes;
using Neo.Application.StreamEventTypes.Activities;
using Neo.Domain.Contracts.ReferentialPointers;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Infrastructure.EventStore.Configurations;
using Neo.Infrastructure.Framework.AspCore;
using Neo.Infrastructure.Framework.Configurations;
using Neo.Infrastructure.Framework.MongoDB.Configurations;
using Neo.Infrastructure.Framework.Redis.Configurations;
using Neo.Infrastructure.Framework.Swagger;
using Neo.Infrastructure.Projection.MongoDB.Configurations;
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
            .With(new CoreBootstrapper())
            .With(new EsDbBootstrapper(Configuration,
                typeof(StreamEventTypeDefined).Assembly,
                typeof(ReferentialPointerDefined).Assembly))
            .With(new MongoBootstraper(Configuration))
            .With(new MongoProjectionBootstraper())
            .With(new RedisBootstraper(Configuration))
            .With(new MassTransitBootstrapper(Configuration,
                  new[] { typeof(DefineStreamEventTypeActivity).Assembly },
                  new[] { typeof(RoutingSlipEventConsumer).Assembly },
                  new[] { typeof(StreamEventTypeStateMachine).Assembly }))
            .With(new SwaggerBootstrapper(Configuration))
            .With(new MvcBootstrapper())
            .Build();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseApplicationExceptionMiddleware();
        app.UseRequestCorrelationMiddleware();
        app.UseSwaggerDocs();
        app.UseCors("CorsPolicy");
        app.UseMvcWithDefaultRoute();
    }
}