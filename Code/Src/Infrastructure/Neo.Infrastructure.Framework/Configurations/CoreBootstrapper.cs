using Microsoft.Extensions.DependencyInjection;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Infrastructure.Framework.Configurations;

public class CoreBootstrapper : IBootstrapper
{
    public void Bootstrap(IServiceCollection services)
    {
        services.AddScoped<ICommandBus, InMemoryCommandBus>();
    }
}