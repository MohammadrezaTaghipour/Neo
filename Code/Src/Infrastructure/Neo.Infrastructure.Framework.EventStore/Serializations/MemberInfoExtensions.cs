using System.Reflection;

namespace Neo.Infrastructure.EventStore.Serializations;

internal static class MemberInfoExtensions
{
    internal static bool IsPropertyWithSetter(this MemberInfo member)
    {
        var property = member as PropertyInfo;
        return property?.GetSetMethod(true) != null;
    }
}