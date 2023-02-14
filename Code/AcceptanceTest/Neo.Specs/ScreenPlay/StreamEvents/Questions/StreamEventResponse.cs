
namespace Neo.Specs.ScreenPlay.StreamEvents.Questions;

public record StreamEventResponse(long Id, Guid LifeStreamId,
    Guid StreamContextId, Guid StreamEventTypeId,
    IReadOnlyCollection<StreamEventMetadaResponseItem> Metadata);

public record StreamEventMetadaResponseItem(string Key, string Value);