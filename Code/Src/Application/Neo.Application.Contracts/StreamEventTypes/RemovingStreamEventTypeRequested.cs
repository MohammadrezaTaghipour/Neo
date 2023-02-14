using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamEventTypes;

public class RemovingStreamEventTypeRequested : BaseRequest
{
    public Guid Id { get; set; }
    public long Version { get; set; }
}
