using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Neo.Infrastructure.EventStore.Serializations;

public class PrivateSetterContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member,
        MemberSerialization memberSerialization)
    {
        var jProperty = base.CreateProperty(member, memberSerialization);
        if (jProperty.Writable)
            return jProperty;
        jProperty.Writable = member.IsPropertyWithSetter();
        return jProperty;
    }
}