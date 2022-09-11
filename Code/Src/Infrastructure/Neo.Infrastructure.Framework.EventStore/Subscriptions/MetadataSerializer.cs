using System.Text.Json;
using Neo.Infrastructure.Framework.Subscriptions.Contexts;

namespace Neo.Infrastructure.EventStore.Subscriptions;

public static class MetadataSerializer
{
    private static readonly JsonSerializerOptions Options;

    static MetadataSerializer()
        => Options = new JsonSerializerOptions(JsonSerializerDefaults.Web);

    public static byte[] Serialize(Metadata evt)
        => JsonSerializer.SerializeToUtf8Bytes(evt, Options);

    public static Metadata Deserialize(ReadOnlyMemory<byte> meta,
        string stream,
        ulong position = 0)
    {
        try
        {
            return JsonSerializer.Deserialize<Metadata>(meta.Span, Options);
        }
        catch (Exception e)
        {
            throw new DeserializationException(stream, "metadata", position, e);
        }
    }
}