using System.Reflection;
using Neo.Application.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Configurations;
using Neo.Infrastructure.Framework.Domain;
using Neo.Infrastructure.Persistence.ES;

namespace ServiceHost.Configurations;

public class NeoBootstrapper : IBootstrapper
{
    public void Bootstrap(IServiceCollection services)
    {
        AddDomainRepository(services, typeof(StreamEventTypeRepository).Assembly);
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
            .Where(a => a.Name.EndsWith("ArgFactory") && !a.IsAbstract && !a.IsInterface)
            .Select(a => new { assignedType = a, serviceTypes = a.GetInterfaces().ToList() })
            .ToList()
            .ForEach(typesToRegister =>
            {
                typesToRegister.serviceTypes
                    .ForEach(typeToRegister => services
                        .AddScoped(typeToRegister, typesToRegister.assignedType));
            });

    }

    static void AddDomainRepository(IServiceCollection services,
        Assembly assembly)
    {
        assembly
            .GetTypes()
            .Where(a => a.Name.EndsWith("Repository") && !a.IsAbstract && !a.IsInterface)
            .Select(a => new { assignedType = a, serviceTypes = a.GetInterfaces().ToList() })
            .ToList()
            .ForEach(typesToRegister =>
            {
                typesToRegister.serviceTypes
                    .ForEach(typeToRegister => services
                        .AddScoped(typeToRegister, typesToRegister.assignedType));
            });
        
    }
}