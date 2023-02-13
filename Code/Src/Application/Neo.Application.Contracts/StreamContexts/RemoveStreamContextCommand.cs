using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamContexts;

public class RemovingStreamContextRequested : BaseRequest
{
    public Guid Id { get; set; }
    public long Version { get; set; }
}