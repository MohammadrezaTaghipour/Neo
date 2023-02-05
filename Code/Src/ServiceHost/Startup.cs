using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Application.ReferentialPointers;
using Neo.Application.StreamContexts;
using Neo.Application.StreamContexts.CourierActivities;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Infrastructure.EventStore.Configurations;
using Neo.Infrastructure.Framework.AspCore;
using Neo.Infrastructure.Framework.Configurations;
using Neo.Infrastructure.Framework.Domain;
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
                typeof(StreamEventTypeDefined).Assembly))
            //.With(new EsDbSubscriptionBootstrapper(Configuration,
            //    typeof(TestEventHandler).Assembly))
            .With(new SwaggerBootstrapper(Configuration))
            .With(new MvcBootstrapper())
            .Build();

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(mt =>
        {
            mt.AddConsumersFromNamespaceContaining<ReferentialPointerConsumer>();
            mt.AddActivitiesFromNamespaceContaining<DefineStreamContextActivity>();
            mt.AddSagaStateMachine<StreamContextStateMachine, StreamContextMachineState>(_ =>
                _.UseInMemoryOutbox())
              .RedisRepository("127.0.0.1");

            mt.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost");
                cfg.ConfigureEndpoints(context);
                //cfg.UseMessageRetry(_ =>
                //{
                //    _.Ignore<BusinessException>();
                //    _.Interval(3, 2000);
                //});
            });
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseApplicationExceptionMiddleware("NEO");
        app.UseSwaggerDocs();
        app.UseCors("CorsPolicy");
        app.UseMvcWithDefaultRoute();
    }
}