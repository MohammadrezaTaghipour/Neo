using Newtonsoft.Json;

namespace Neo.Infrastructure.EventStore.Serializations;

public interface IEventSerializer
{
    object Deserialize(string serializedString, Type typeOfEvent);
}

public class EventSerializer : IEventSerializer
{
    public object Deserialize(string serializedString, Type typeOfEvent)
    {
        return JsonConvert.DeserializeObject(serializedString, typeOfEvent,
            new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
    }
}