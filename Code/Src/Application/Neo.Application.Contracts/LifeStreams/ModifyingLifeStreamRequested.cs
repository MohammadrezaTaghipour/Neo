using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.LifeStreams;

public class ModifyingLifeStreamRequested : BaseRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public long Version { get; set; }
}

public class ModifyingLifeStreamRequestExecuted
{
    public Guid Id { get; set; }
}