using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamEventTypes;

public class ModifyingStreamEventTypeRequested : BaseRequest
{
    public ModifyingStreamEventTypeRequested()
    {
        Metadata = new List<StreamEventTypeMetadataRequestItem>();
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public IReadOnlyList<StreamEventTypeMetadataRequestItem> Metadata { get; set; }
    public long Version { get; set; }
}
