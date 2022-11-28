using System.Reflection;
using Neo.Application.Query.StreamEventTypes;
using Neo.Application.StreamEventTypes;
using Neo.Application.StreamEventTypes.Validators;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Configurations;
using Neo.Infrastructure.Persistence.ES;

namespace ServiceHost.Configurations;

public class NeoBootstrapper : IBootstrapper
{
    public void Bootstrap(IServiceCollection services)
    {
        AddDomainRepository(services, typeof(StreamEventTypeRepository).Assembly);
        AddArgFactories(services, typeof(StreamEventTypeArgFactory).Assembly);
        AddApplicationServices(services, typeof(StreamEventTypeApplicationService).Assembly);
        AddQueryServices(services, typeof(StreamEventTypeQueryService).Assembly);
        AddCommandValidators(services, typeof(StreamEventTypeCommandValidators).Assembly);
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

    static void AddQueryServices(IServiceCollection services,
        Assembly assembly)
    {
        assembly
            .GetTypes()
            .Where(a => a.Name.EndsWith("QueryService") && !a.IsAbstract && !a.IsInterface)
            .Select(a => new { assignedType = a, serviceTypes = a.GetInterfaces().ToList() })
            .ToList()
            .ForEach(typesToRegister =>
            {
                typesToRegister.serviceTypes
                    .ForEach(typeToRegister => services
                        .AddScoped(typeToRegister, typesToRegister.assignedType));
            });
    }

    static void AddApplicationServices(IServiceCollection services,
        Assembly assembly)
    {
        assembly.GetTypes()
             .Where(t => t.GetTypeInfo()
                 .ImplementedInterfaces.Any(
                     i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IApplicationService<>)))
             .Select(a => new { assignedType = a, serviceTypes = a.GetInterfaces().ToList() })
             .ToList()
             .ForEach(typesToRegister =>
             {
                 typesToRegister.serviceTypes
                     .ForEach(typeToRegister => services
                         .AddScoped(typeToRegister, typesToRegister.assignedType));
             });
    }

    static void AddCommandValidators(IServiceCollection services,
        Assembly assembly)
    {
        assembly.GetTypes()
             .Where(t => t.GetTypeInfo()
                 .ImplementedInterfaces.Any(
                     i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandValidator<>)))
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