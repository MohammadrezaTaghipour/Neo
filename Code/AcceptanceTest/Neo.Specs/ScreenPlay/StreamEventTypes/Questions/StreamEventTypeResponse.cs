namespace Neo.Specs.ScreenPlay.StreamEventTypes.Questions;

public record StreamEventTypeResponse(Guid? Id,
    string? Title,
    bool? Removed,
    IReadOnlyCollection<StreamEventTypeMetadataResponseItem>? Metadata);

public record StreamEventTypeMetadataResponseItem(string Title);

public record StatusResponse(bool Completed, string ErrorCode, string ErrorMessage)
{
    public bool Faulted => !string.IsNullOrEmpty(ErrorCode) ||
                           !string.IsNullOrEmpty(ErrorMessage);
};