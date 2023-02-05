using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.ReferentialPointers;

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

public class SyncingReferentialPointersRequest
{
    public SyncingReferentialPointersRequest()
    {
        CurrentState = new();
        NextState = new();
    }

    public Guid Id { get; set; }
    public ReferentialPointerContainer CurrentState { get; set; }
    public ReferentialPointerContainer NextState { get; set; }
}

public class StreamContextReferentialPointersSynced
{
    public Guid Id { get; set; }
}

public class DefiningStreamContextFaulted
{
    public Guid Id { get; set; }
}