using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamContexts;

public class RemoveStreamContextRequested : BaseRequest
{
    public Guid Id { get; set; }
    public long Version { get; set; }
}