using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.LifeStreams;

public class AppendStreamEventCommand : BaseRequest
{
    public long Id { get; set; }
    public Guid LifeStreamId { get; set; }
    public Guid StreamContextId { get; set; }
    public Guid StreamEventTypeId { get; set; }
    public IReadOnlyCollection<StreamEventMetadaCommandItem> Metadata { get; set; }
}