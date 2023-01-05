
using Neo.Specs.ScreenPlay.StreamEvents.Questions;

namespace Neo.Specs.ScreenPlay.LifeStreams.Questions;

public record LifeStreamResponse(string Title,
    string Description, bool Removed,
    IReadOnlyCollection<StreamEventResponse> StreamEvents);
