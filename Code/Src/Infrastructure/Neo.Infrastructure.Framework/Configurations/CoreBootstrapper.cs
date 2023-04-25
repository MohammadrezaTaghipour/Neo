using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.AspCore;
using Neo.Infrastructure.Framework.Domain;
using Neo.Infrastructure.Framework.Persistence;
using Neo.Infrastructure.Framework.Projections;
using Neo.Infrastructure.Framework.Serializations;

namespace Neo.Infrastructure.Framework.Configurations;

public class CoreBootstrapper : IBootstrapper
{
    public void Bootstrap(IServiceCollection services)
    {
        services.AddScoped<ICommandBus, InMemoryCommandBus>();
        services.AddScoped<IAggregateReader, AggregateReader>();
        services.AddScoped<IErrorResponseBuilder, ErrorResponseBuilder>();
        services.AddSingleton<IEventAggregator, EventAggregator>();
        services.AddSingleton<IMessageSerializer, NewtonsoftMessageSerializer>();

        services.AddScoped<IDominEventProjectorDispatcher, InMemoryDominEventProjectorDispatcher>();
        services.AddScoped<RequestCorrelationFilterAttribute>();
    }
}