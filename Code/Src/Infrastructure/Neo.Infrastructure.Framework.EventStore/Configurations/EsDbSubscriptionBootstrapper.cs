using System.Reflection;
using EventStore.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Neo.Infrastructure.EventStore.Extensions;
using Neo.Infrastructure.EventStore.Subscriptions;
using Neo.Infrastructure.Framework.Configurations;
using Neo.Infrastructure.Framework.Subscriptions;
using Neo.Infrastructure.Framework.Subscriptions.Consumers;

namespace Neo.Infrastructure.EventStore.Configurations;

public class EsDbSubscriptionBootstrapper : IBootstrapper
{
    private readonly IConfiguration _configuration;
    private readonly Assembly _assemblyWithEventHandlers;

    public EsDbSubscriptionBootstrapper(IConfiguration configuration,
        Assembly assemblyWithEventHandlers)
    {
        _configuration = configuration;
        _assemblyWithEventHandlers = assemblyWithEventHandlers;
    }

    public void Bootstrap(IServiceCollection services)
    {
        var esDbOption = _configuration.GetSection("esDb").Get<EsDbOption>();

        services.AddSingleton(_ => new PersistentSubscriptionOptions(esDbOption.SubscriptionId)
        {
            ResolveLinkTos = true,
            BufferSize = 1,
            SubscriptionSettings = new PersistentSubscriptionSettings(true, Position.Start)
        });
        services.AddSingleton<IMessageSubscription, EventStorePersistentSubscriber>();
        services.AddSingleton<IMessageConsumer, DefaultMessageConsumer>();
        AddEventHandlers(services, _assemblyWithEventHandlers);

        services.AddSingleton(_ =>
        {
            var esDbClient = _.GetRequiredService<EventStoreClient>();
            var settings = esDbClient.GetSettings().Copy();
            var opSettings = settings.OperationOptions.Clone();
            settings.OperationOptions = opSettings;

            return new EventStorePersistentSubscriptionsClient(settings);
        });

        services.AddSingleton<IHostedService>(_ => new SubscriptionHostedService(
            _.GetRequiredService<IMessageSubscription>(),
            _.GetRequiredService<ILoggerFactory>()
        ));
    }

    static void AddEventHandlers(IServiceCollection services,
        Assembly assembly)
    {
        assembly
            .GetTypes()
            .Where(a => a.Name.EndsWith("EventHandler") && !a.IsAbstract && !a.IsInterface)
            .Select(a => new { assignedType = a, serviceTypes = a.GetInterfaces().ToList() })
            .ToList()
            .ForEach(typesToRegister =>
            {
                typesToRegister.serviceTypes
                    .ForEach(typeToRegister => services
                        .AddSingleton(typeToRegister, typesToRegister.assignedType));
            });
    }
}