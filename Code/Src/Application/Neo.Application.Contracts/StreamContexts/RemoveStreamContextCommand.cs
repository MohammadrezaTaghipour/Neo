using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamContexts;

public class RemovingStreamContextRequested : IRequest
{
    public string? RequestId { get; set; }
    public Guid Id { get; set; }
    public long Version { get; set; }
}