using System.Reflection;
using Neo.Application.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Configurations;
using Neo.Infrastructure.Framework.Domain;

namespace ServiceHost.Configurations;

public class NeoBootstrapper : IBootstrapper
{
    public void Bootstrap(IServiceCollection services)
    {
        AddApplicationServices(services, typeof(StreamEventTypeService).Assembly);
        AddArgFactories(services, typeof(StreamEventTypeArgFactory).Assembly);
    }

    static void AddApplicationServices(IServiceCollection services,
        Assembly assembly)
    {
        assembly
            .GetTypes()
            .Where(item => item.GetInterfaces()
                               .Where(i => i.IsGenericType)
                               .Any(i => i.GetGenericTypeDefinition() ==
                                         typeof(IApplicationService<>))
                           && !item.IsAbstract && !item.IsInterface)
            .ToList()
            .ForEach(assignedTypes =>
            {
                var serviceType = assignedTypes
                    .GetInterfaces()
                    .First(i => i.GetGenericTypeDefinition() ==
                                typeof(IApplicationService<>));
                services.AddScoped(serviceType, assignedTypes);
            });
    }

    static void AddArgFactories(IServiceCollection services,
        Assembly assembly)
    {
        assembly
            .GetTypes()
            .Where(item => item.GetInterfaces()
                               .Where(i => i.IsGenericType)
                               .Any(i => i.GetGenericTypeDefinition() ==
                                         typeof(IDomainArgFactory))
                           && !item.IsAbstract && !item.IsInterface)
            .ToList()
            .ForEach(assignedTypes =>
            {
                var serviceType = assignedTypes
                    .GetInterfaces()
                    .First(i => i.GetGenericTypeDefinition() ==
                                typeof(IDomainArgFactory));
                services.AddScoped(serviceType, assignedTypes);
            });
    }
}