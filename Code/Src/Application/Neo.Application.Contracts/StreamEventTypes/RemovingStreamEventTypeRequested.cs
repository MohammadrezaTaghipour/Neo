using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamEventTypes;

public class RemovingStreamEventTypeRequested : IRequest
{
    public string? RequestId { get; set; }
    public Guid Id { get; set; }
    public long Version { get; set; }
}
