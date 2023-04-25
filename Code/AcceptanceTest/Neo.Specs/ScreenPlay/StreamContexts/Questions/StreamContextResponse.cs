
namespace Neo.Specs.ScreenPlay.StreamContexts.Questions;

public record StreamContextResponse(Guid Id,
    string Title,
    string Description, bool Removed,
    IReadOnlyCollection<Guid> StreamEventTypes);