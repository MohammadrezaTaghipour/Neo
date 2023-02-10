using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Application.ReferentialPointers;
using Neo.Application.StreamContexts;
using Neo.Application.StreamContexts.Activities;
using Neo.Application.StreamEventTypes;
using Neo.Application.StreamEventTypes.Activities;
using Neo.Application.StreamEventTypes.Validators;
using Neo.Domain.Contracts.ReferentialPointers;
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
                typeof(StreamEventTypeDefined).Assembly,
                typeof(ReferentialPointerDefined).Assembly))
            //.With(new EsDbSubscriptionBootstrapper(Configuration,
            //    typeof(TestEventHandler).Assembly))
            .With(new SwaggerBootstrapper(Configuration))
            .With(new MvcBootstrapper())
            .Build();


        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<DefineStreamEventTypeCommandValidator>();

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(mt =>
        {
            mt.AddActivities(typeof(DefineStreamContextActivity).Assembly);

            mt.AddSagaStateMachine<StreamContextStateMachine, StreamContextMachineState>(_ =>
                _.UseInMemoryOutbox())
              .RedisRepository("127.0.0.1");
            mt.AddSagaStateMachine<StreamEventTypeStateMachine, StreamEventTypeMachineState>(_ =>
            {
                _.ConcurrentMessageLimit = 1;
                _.UseInMemoryOutbox();
                _.UseMessageRetry(r => r.Intervals(2, 1000));
            })
            .RedisRepository("127.0.0.1");

            //(typeof(StreamEventTypeStateMachineDefinition))
            //  .RedisRepository("127.0.0.1");

            mt.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost");
                cfg.ConfigureEndpoints(context);
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