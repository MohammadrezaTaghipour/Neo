
namespace Neo.Specs.ScreenPlay.StreamContexts.Questions;

public record StreamContextResponse(string Title, string Description,
    IReadOnlyCollection<Guid> StreamEventTypes, bool Removed);