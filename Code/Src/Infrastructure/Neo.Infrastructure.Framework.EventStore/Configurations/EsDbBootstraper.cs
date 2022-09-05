using System.Reflection;
using EventStore.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neo.Infrastructure.EventStore.Serializations;
using Neo.Infrastructure.Framework.Configurations;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Infrastructure.EventStore.Configurations;

public class EsDbBootstrapper : IBootstrapper
{
    private readonly Assembly[] _assembliesWithEvents;
    private readonly IConfiguration _configuration;

    public EsDbBootstrapper(IConfiguration configuration,
        params Assembly[] assembliesWithEvents)
    {
        _configuration = configuration;
        _assembliesWithEvents = assembliesWithEvents;
    }

    public void Bootstrap(IServiceCollection services)
    {
        var esDbOption = _configuration.GetSection("esDb").Get<EsDbOption>();

        services.AddScoped(_ =>
        {
            var settings = EventStoreClientSettings
                .Create(esDbOption.DbConnectionString);
            var client = new EventStoreClient(settings);
            return client;
        });
        services.AddSingleton(_ =>
        {
            var mapper = new DomainEventTypeMapper();
            mapper.RegisterKnownEventTypes(_assembliesWithEvents);
            return mapper;
        });
        services.AddScoped(typeof(IEventSourcedRepository<,,>), typeof(EsdbRepository<,,>));
        services.AddScoped<IDomainEventFactory, DomainEventFactory>();
        services.AddScoped<IEventSerializer, EventSerializer>();
    }
}