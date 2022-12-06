
using Neo.Specs.ScreenPlay.StreamEventTypes.Questions;

namespace Neo.Specs.ScreenPlay.LifeStreams.Questions;

public record LifeStreamResponse(string Title, string description,
    IReadOnlyCollection<LifeStreamResponseItem> Metadata, bool Deleted);

public record LifeStreamResponseItem();
