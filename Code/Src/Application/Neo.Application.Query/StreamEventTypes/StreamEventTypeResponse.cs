namespace Neo.Application.Query.StreamEventTypes;

public record StreamEventTypeResponse(Guid? Id, string? Title,
    bool? Removed,
    IReadOnlyCollection<StreamEventTypeMetadataResponse> Metadata)
{
}


public record StreamEventTypeMetadataResponse(string Title);