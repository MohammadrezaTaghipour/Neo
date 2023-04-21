namespace Neo.Application.Query.LifeStreams;

public record LifeStreamResponse(
    Guid? Id,
    string? Title,
    string? Description,
    bool? Removed,
    IReadOnlyCollection<LifeStreamEventResponse>? StreamEvents,
    StatusResponse Status)
{
    public static LifeStreamResponse CreateFaulted(StatusResponse response)
    {
        return new LifeStreamResponse(null, null,
            null, null, null, response);
    }
}


public record LifeStreamEventResponse(
    long Id,
    Guid LifeStreamId,
    Guid StreamContextId,
    Guid StreamEventTypeId,
    IReadOnlyCollection<LifeStreamEventMetadaResponse> Metadata);

public record LifeStreamEventMetadaResponse(string Key, string Value);