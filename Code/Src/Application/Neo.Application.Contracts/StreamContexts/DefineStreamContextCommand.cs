using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamContexts;

public class DefiningStreamContextRequested : BaseCommand
{
    public DefiningStreamContextRequested()
    {
        StreamEventTypes = new List<StreamEventTypeCommandItem>();
    }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IReadOnlyCollection<StreamEventTypeCommandItem> StreamEventTypes { get; set; }
}

public class DefiningStreamContextRequestExecuted
{
    public Guid Id { get; set; }
}

public class SyncingStreamContextReferentialPointersRequest
{
    public Guid Id { get; set; }
}

public class StreamContextReferentialPointersSynced
{
    public Guid Id { get; set; } 
}