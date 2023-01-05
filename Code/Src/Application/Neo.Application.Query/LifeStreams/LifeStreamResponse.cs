namespace Neo.Application.Query.LifeStreams;

public record LifeStreamResponse(Guid Id,
    string Title, string Description, bool Removed,
    IReadOnlyCollection<LifeStreamEventResponse> StreamEvents);


public record LifeStreamEventResponse(long Id, 
    Guid LifeStreamId, Guid StreamContextId, Guid StreamEventTypeId,
       IReadOnlyCollection<LifeStreamEventMetadaResponse> Metadata);

public record LifeStreamEventMetadaResponse(string Key, string Value);