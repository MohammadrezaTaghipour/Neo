
using Neo.Specs.ScreenPlay.StreamEvents.Questions;
using Neo.Specs.ScreenPlay.StreamEventTypes.Questions;

namespace Neo.Specs.ScreenPlay.LifeStreams.Questions;

public record LifeStreamResponse(Guid? Id,
    string? Title,
    string? Description,
    bool? Removed,
    IReadOnlyCollection<StreamEventResponse>? StreamEvents,
    StatusResponse Status);
