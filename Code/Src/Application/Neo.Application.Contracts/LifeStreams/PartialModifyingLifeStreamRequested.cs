using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.LifeStreams;

public class PartialModifyingLifeStreamRequested : BaseRequest
{
    public PartialModifyingLifeStreamRequested()
    {
        Metadata = new List<StreamEventMetadaRequestItem>();
    }

    public long Id { get; set; }
    public LifeStreamPartialModificationOperationType OperationType { get; set; }
    public Guid LifeStreamId { get; set; }
    public Guid StreamContextId { get; set; }
    public Guid StreamEventTypeId { get; set; }
    public IReadOnlyCollection<StreamEventMetadaRequestItem> Metadata { get; set; }
}


public enum LifeStreamPartialModificationOperationType
{
    AppendStreamEvent,
    RemoveStreamEvent,
}


public class StreamEventMetadaRequestItem
{
    public string Key { get; set; }
    public string Value { get; set; }
}
