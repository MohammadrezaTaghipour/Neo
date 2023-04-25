namespace Neo.Application.Query.StreamContexts;

public record StreamContextResponse(Guid? Id, string? Title,
    string? Description, bool? Removed, IReadOnlyCollection<Guid>? StreamEventTypes)
{
}