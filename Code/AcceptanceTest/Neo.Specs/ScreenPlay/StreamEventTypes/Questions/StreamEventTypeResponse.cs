namespace Neo.Specs.ScreenPlay.StreamEventTypes.Questions;

public record StreamEventTypeResponse(string? Title,
    bool? Removed,
    IReadOnlyCollection<StreamEventTypeMetadataResponseItem>? Metadata,
    StatusResponse Status);

public record StreamEventTypeMetadataResponseItem(string Title);

public record StatusResponse(bool Completed, string ErrorCode, string ErrorMessage)
{
    public bool Faulted => !string.IsNullOrEmpty(ErrorCode) ||
                           !string.IsNullOrEmpty(ErrorMessage);
};