using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamEventTypes;

public class DefiningStreamEventTypeRequested : BaseRequest
{
    public DefiningStreamEventTypeRequested()
    {
        Metadata = new List<StreamEventTypeMetadataRequestItem>();
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public IReadOnlyCollection<StreamEventTypeMetadataRequestItem> Metadata { get; set; }
}
