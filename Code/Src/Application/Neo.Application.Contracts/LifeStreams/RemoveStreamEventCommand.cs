using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.LifeStreams;

public class RemoveStreamEventCommand : BaseRequest
{
    public long Id { get; set; }
    public Guid LifeStreamId { get; set; }
}