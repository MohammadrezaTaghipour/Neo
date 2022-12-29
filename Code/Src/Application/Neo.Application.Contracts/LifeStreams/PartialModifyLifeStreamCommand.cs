using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.LifeStreams;

public class PartialModifyLifeStreamCommand : BaseCommand
{
    public PartialModifyLifeStreamCommand()
    {
        Metadata = new List<StreamEventMetadaCommandItem>();
    }

    public long Id { get; set; }
    public LifeStreamPartialModificationOperationType OperationType { get; set; }
    public Guid LifeStreamId { get; set; }
    public Guid StreamContextId { get; set; }
    public Guid StreamEventTypeId { get; set; }
    public IReadOnlyCollection<StreamEventMetadaCommandItem> Metadata { get; set; }
}


public enum LifeStreamPartialModificationOperationType
{
    AppendStreamEvent,
    RemoveStreamEvent,
}


public class StreamEventMetadaCommandItem
{
    public string Key { get; set; }
    public string Value { get; set; }
}
