namespace Neo.Application.Query.StreamEventTypes;

public record StreamEventTypeResponse(Guid? Id, string? Title,
    bool? Removed, IReadOnlyCollection<StreamEventTypeMetadataResponse>? Metadata,
    StatusResponse Status);

public record StreamEventTypeMetadataResponse(string Title);

public record StatusResponse(bool Completed, string ErrorCode, string ErrorMessage);