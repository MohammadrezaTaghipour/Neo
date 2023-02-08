using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.LifeStreams;

public class DefineLifeStreamCommand : BaseRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
