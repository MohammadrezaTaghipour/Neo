
using Neo.Infrastructure.Projection.MongoDB;

namespace Neo.Application.Query.StreamContexts;

public record StreamContextResponse(Guid? Id, string? Title,
    string? Description, bool? Removed, IReadOnlyCollection<Guid>? StreamEventTypes,
    StatusResponse Status)
{
    public static StreamContextResponse CreateFaulted(StatusResponse response)
    {
        return new StreamContextResponse(null, null,
            null, null, null, response);
    }
}