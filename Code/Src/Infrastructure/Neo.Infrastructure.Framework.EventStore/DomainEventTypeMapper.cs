using System.Reflection;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Infrastructure.EventStore;

public class DomainEventTypeMapper
{
    private Dictionary<string, Type> _typeMap = new();

    public void RegisterKnownEventTypes(params Assembly[] assembliesWithEvents)
    {
        var assemblies = assembliesWithEvents.Union(new List<Assembly> { GetType().Assembly });
        _typeMap = GetSubtypesOf<IDomainEvent>(assemblies).ToDictionary(t => t.Name);

        static IReadOnlyCollection<Type> GetSubtypesOf<T>(IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .SelectMany(assembly => assembly.GetExportedTypes()
                    .Where(t => typeof(T).IsAssignableFrom(t)))
                .ToList();
        }
    }

    public Type GetType(string typeName)
    {
        if (!_typeMap.TryGetValue(typeName, out var type))
            throw new Exception($"Unregistered type exception: {typeName}");
        return type;
    }
}