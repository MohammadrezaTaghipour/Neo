using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.LifeStreams;

public class RemovingLifeStreamRequested : BaseRequest
{
    public Guid Id { get; set; }
    public long Version { get; set; }
}

public class RemovingLifeStreamRequestExecuted
{
    public Guid Id { get; set; }
}