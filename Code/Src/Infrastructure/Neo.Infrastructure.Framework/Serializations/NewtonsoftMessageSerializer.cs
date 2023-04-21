using Newtonsoft.Json;

namespace Neo.Infrastructure.Framework.Serializations;

public class NewtonsoftMessageSerializer : IMessageSerializer
{
    public T? Deserialize<T>(string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException(nameof(message));

        return JsonConvert.DeserializeObject<T>(message);
    }

    public string Serialize<T>(T message)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        return JsonConvert.SerializeObject(message);
    }
}