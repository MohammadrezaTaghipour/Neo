using Microsoft.Extensions.DependencyInjection;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Configurations;
using Neo.Infrastructure.Framework.Domain;
using Neo.Infrastructure.Framework.Projections;
using Neo.Infrastructure.Projection.MongoDB.StreamEventTypes;
using System.Reflection;

namespace Neo.Infrastructure.Projection.MongoDB.Configurations;

public class MongoProjectionBootstraper : IBootstrapper
{

    public void Bootstrap(IServiceCollection services)
    {
        AddProjectors(services, typeof(StreamEventTypeProjections).Assembly);
    }

    static void AddProjectors(IServiceCollection services,
       Assembly assembly)
    {
        assembly.GetTypes()
             .Where(t => t.GetTypeInfo()
                 .ImplementedInterfaces.Any(
                     i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDominEventProjector<>)))
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
