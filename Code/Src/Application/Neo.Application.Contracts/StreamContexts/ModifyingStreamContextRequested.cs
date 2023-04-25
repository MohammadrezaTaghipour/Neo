using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamContexts;

public class ModifyingStreamContextRequested : IRequest
{
    public ModifyingStreamContextRequested()
    {
        StreamEventTypes = new List<StreamEventTypeRequestItem>();
    }

    public string? RequestId { get; set; }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IReadOnlyCollection<StreamEventTypeRequestItem> StreamEventTypes { get; set; }
    public long Version { get; set; }
}
