using System.Reflection;
using System.Collections;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Infrastructure.Framework.Configurations;

public class AssemblyScanner : IEnumerable<AssemblyScanner.AssemblyScanResult>
{
    readonly IEnumerable<Type> _types;

    /// <summary>
    /// Creates a scanner that works on a sequence of types.
    /// </summary>
    public AssemblyScanner(IEnumerable<Type> types)
    {
        _types = types;
    }

    /// <summary>
    /// Finds all the validators in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan</param>
    /// <param name="includeInternalTypes">Whether to include internal validators in the search as well as public validators. The default is false.</param>
    public static AssemblyScanner FindApplicationServicesInAssembly(Assembly assembly, bool includeInternalTypes = false)
    {
        return new AssemblyScanner(includeInternalTypes ? assembly.GetTypes() : assembly.GetExportedTypes());
    }

    /// <summary>
    /// Finds all the validators in the specified assemblies.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan</param>
    /// <param name="includeInternalTypes">Whether to include internal validators as well as public validators. The default is false.</param>
    public static AssemblyScanner FindValidatorsInAssemblies(IEnumerable<Assembly> assemblies,
        bool includeInternalTypes = false)
    {
        var types = assemblies.SelectMany(x => includeInternalTypes ? x.GetTypes() : x.GetExportedTypes()).Distinct();
        return new AssemblyScanner(types);
    }

    /// <summary>
    /// Finds all the validators in the assembly containing the specified type.
    /// </summary>
    public static AssemblyScanner FindValidatorsInAssemblyContaining<T>()
    {
        return FindApplicationServicesInAssembly(typeof(T).Assembly);
    }

    /// <summary>
    /// Finds all the validators in the assembly containing the specified type.
    /// </summary>
    public static AssemblyScanner FindValidatorsInAssemblyContaining(Type type)
    {
        return FindApplicationServicesInAssembly(type.Assembly);
    }

    private IEnumerable<AssemblyScanResult> Execute()
    {
        var openGenericType = typeof(IApplicationService<>);

        var query = from type in _types
            where !type.IsAbstract && !type.IsGenericTypeDefinition
            let interfaces = type.GetInterfaces()
            let genericInterfaces =
                interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericType)
            let matchingInterface = genericInterfaces.FirstOrDefault()
            where matchingInterface != null
            select new AssemblyScanResult(matchingInterface, type);

        return query;
    }

    /// <summary>
    /// Performs the specified action to all of the assembly scan results.
    /// </summary>
    public void ForEach(Action<AssemblyScanResult> action)
    {
        foreach (var result in this)
        {
            action(result);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public IEnumerator<AssemblyScanResult> GetEnumerator()
    {
        return Execute().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Result of performing a scan.
    /// </summary>
    public class AssemblyScanResult
    {
        /// <summary>
        /// Creates an instance of an AssemblyScanResult.
        /// </summary>
        public AssemblyScanResult(Type interfaceType, Type validatorType)
        {
            InterfaceType = interfaceType;
            ValidatorType = validatorType;
        }

        /// <summary>
        /// Validator interface type, eg IValidator&lt;Foo&gt;
        /// </summary>
        public Type InterfaceType { get; private set; }

        /// <summary>
        /// Concrete type that implements the InterfaceType, eg FooValidator.
        /// </summary>
        public Type ValidatorType { get; private set; }
    }
}