using Microsoft.Extensions.DependencyInjection;

namespace Neo.Infrastructure.Framework.Configurations;

public interface IBootstrapper
{
    void Bootstrap(IServiceCollection services);
}