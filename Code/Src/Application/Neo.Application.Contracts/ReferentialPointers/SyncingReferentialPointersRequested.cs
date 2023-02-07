using Neo.Infrastructure.Framework.ReferentialPointers;

namespace Neo.Application.Contracts.ReferentialPointers;

public class SyncingReferentialPointersRequested
{
    public SyncingReferentialPointersRequested()
    {
        CurrentState = new();
        NextState = new();
    }

    public Guid Id { get; set; }
    public ReferentialPointerContainer CurrentState { get; set; }
    public ReferentialPointerContainer NextState { get; set; }
}
