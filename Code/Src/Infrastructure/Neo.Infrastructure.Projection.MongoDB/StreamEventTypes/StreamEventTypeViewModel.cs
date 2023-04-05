using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Neo.Infrastructure.Projection.MongoDB.StreamEventTypes;

public class StreamEventTypeProjectionModel
{
    public StreamEventTypeProjectionModel(
        Guid id,
        string title,
        bool removed,
        long version,
        IReadOnlyCollection<StreamEventTypeMetadataProjectionModel> metadata)
    {
        Id = id;
        Title = title;
        Removed = removed;
        Version = version;
        Metadata = metadata;
    }

    [BsonId]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool Removed { get; set; }
    public long Version { get; set; }
    public IReadOnlyCollection<StreamEventTypeMetadataProjectionModel> Metadata { get; set; }
}


public record StreamEventTypeMetadataProjectionModel(string Title);
