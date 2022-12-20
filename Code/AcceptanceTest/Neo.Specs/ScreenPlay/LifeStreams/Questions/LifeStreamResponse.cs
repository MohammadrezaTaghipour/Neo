
namespace Neo.Specs.ScreenPlay.LifeStreams.Questions;

public record LifeStreamResponse(string Title, string Description,
    IReadOnlyCollection<LifeStreamResponseItem> ParentStream, bool Removed);

public record LifeStreamResponseItem();
