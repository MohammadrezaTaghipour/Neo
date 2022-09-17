using Microsoft.Extensions.DependencyInjection;

namespace Neo.Infrastructure.Framework.Configurations;

public class BootstrapBuilder
{
    private readonly IServiceCollection _services;
    private readonly List<IBootstrapper> _bootstrappers = new();

    private BootstrapBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public static BootstrapBuilder GetInstance(IServiceCollection services)
    {
        return new BootstrapBuilder(services);
    }

    public BootstrapBuilder With(IBootstrapper bootstrapper)
    {
        _bootstrappers.Add(bootstrapper);
        return this;
    }

    public void Build()
    {
        _bootstrappers.ForEach(a => a.Bootstrap(_services));
    }
}