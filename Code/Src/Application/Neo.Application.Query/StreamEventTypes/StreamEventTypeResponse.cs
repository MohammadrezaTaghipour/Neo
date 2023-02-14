
namespace Neo.Application.Query.StreamEventTypes;

public record StreamEventTypeResponse(Guid? Id, string? Title,
    bool? Removed, IReadOnlyCollection<StreamEventTypeMetadataResponse>? Metadata,
    StatusResponse Status)
{
    public static StreamEventTypeResponse CreateFaulted(StatusResponse response)
    {
        return new StreamEventTypeResponse(null,
            null, null, null, response);
    }
}


public record StreamEventTypeMetadataResponse(string Title);
