
using Neo.Specs.ScreenPlay.StreamEventTypes.Questions;

namespace Neo.Specs.ScreenPlay.StreamContexts.Questions;

public record StreamContextResponse(string Title,
    string Description, bool Removed,
    IReadOnlyCollection<Guid> StreamEventTypes,
    StatusResponse Status);