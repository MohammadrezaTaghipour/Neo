namespace Neo.Specs.ScreenPlay.StreamEventTypes.Questions;

public record StreamEventTypeResponse(string Title, 
    IReadOnlyCollection<StreamEventTypeMetadataResponseItem> Metadata, bool Removed);

public record StreamEventTypeMetadataResponseItem(string Title);