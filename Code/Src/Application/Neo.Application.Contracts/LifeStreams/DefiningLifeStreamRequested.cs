using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.LifeStreams;

public class DefiningLifeStreamRequested : BaseRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}


public class DefiningLifeStreamRequestExecuted
{
    public Guid Id { get; set; }
}
