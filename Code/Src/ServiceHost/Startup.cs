using Neo.Application;
using Neo.Application.StreamEventTypes;
using Neo.Application.StreamEventTypes.Activities;
using Neo.Domain.Contracts.ReferentialPointers;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Infrastructure.EventStore.Configurations;
using Neo.Infrastructure.Framework.AspCore;
using Neo.Infrastructure.Framework.Configurations;
using Neo.Infrastructure.Framework.Swagger;
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
            //.With(new EsDbSubscriptionBootstrapper(Configuration,
            //    typeof(TestEventHandler).Assembly))
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
        app.UseSwaggerDocs();
        app.UseCors("CorsPolicy");
        app.UseMvcWithDefaultRoute();
    }
}