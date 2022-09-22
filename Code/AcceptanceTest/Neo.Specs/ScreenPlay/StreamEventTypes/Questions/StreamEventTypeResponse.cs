namespace Neo.Specs.ScreenPlay.StreamEventTypes.Questions;

public record StreamEventTypeResponse(string Title, 
    IReadOnlyCollection<StreamEventTypeMetadataResponseItem> Metadata);

public record StreamEventTypeMetadataResponseItem(string Title);