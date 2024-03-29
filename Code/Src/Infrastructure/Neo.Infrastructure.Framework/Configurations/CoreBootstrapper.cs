using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.AspCore;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Infrastructure.Framework.Configurations;

public class CoreBootstrapper : IBootstrapper
{
    public void Bootstrap(IServiceCollection services)
    {
        services.AddScoped<ICommandBus, InMemoryCommandBus>();
        services.AddScoped<IAggregateReader, AggregateReader>();
        services.AddScoped<IErrorResponseBuilder, ErrorResponseBuilder>();
    }
}