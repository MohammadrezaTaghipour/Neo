namespace Neo.Infrastructure.Framework.Serializations;

public interface IMessageSerializer
{
    T? Deserialize<T>(string message);
    string Serialize<T>(T message);
}
