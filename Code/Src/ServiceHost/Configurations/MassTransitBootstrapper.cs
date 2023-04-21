using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Neo.Application;
using Neo.Application.LifeStreams;
using Neo.Application.StreamContexts;
using Neo.Application.StreamEventTypes;
using Neo.Infrastructure.Framework.Configurations;
using System.Reflection;

namespace ServiceHost.Configurations;

public class MassTransitBootstrapper : IBootstrapper
{
    private readonly IConfiguration _configuration;
    private readonly Assembly[] _assembliesWithActivity;
    private readonly Assembly[] _assembliesWithConsumer;
    private readonly Assembly[] _assembliesWithSagaStateMachine;

    public MassTransitBootstrapper(IConfiguration configuration,
        Assembly[] assembliesWithActivity,
        Assembly[] assembliesWithConsumer,
        Assembly[] assembliesWithSagaStateMachine)
    {
        _configuration = configuration;
        _assembliesWithActivity = assembliesWithActivity;
        _assembliesWithConsumer = assembliesWithConsumer;
        _assembliesWithSagaStateMachine = assembliesWithSagaStateMachine;
    }

    public void Bootstrap(IServiceCollection services)
    {
        services.Configure<MassTransitOptions>(_configuration.GetSection("massTransit"));
        var option = _configuration.GetSection("massTransit").Get<MassTransitOptions>();

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(mt =>
        {
            mt.AddActivities(_assembliesWithActivity);
            mt.AddConsumers(_assembliesWithConsumer);

            mt.AddSagaStateMachine<StreamContextStateMachine, StreamContextMachineState>(_ =>
            {
                _.ConcurrentMessageLimit = 1;
                _.UseInMemoryOutbox();
            })
              .RedisRepository(option.RedisRepository);
            mt.AddSagaStateMachine<StreamEventTypeStateMachine, StreamEventTypeMachineState>(_ =>
            {
                _.ConcurrentMessageLimit = 1;
                _.UseInMemoryOutbox();
            })
              .RedisRepository(option.RedisRepository);
            mt.AddSagaStateMachine<LifeStreamStateMachine, LifeStreamMachineState>(_ =>
            {
                _.ConcurrentMessageLimit = 1;
                _.UseInMemoryOutbox();
            })
              .RedisRepository(option.RedisRepository);

            mt.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(option.Host));
                cfg.ConfigureEndpoints(context);
                cfg.PrefetchCount = option.PrefetchCount;
            });
        });
    }
}

