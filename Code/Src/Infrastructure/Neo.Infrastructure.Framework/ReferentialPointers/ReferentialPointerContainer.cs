

namespace Neo.Infrastructure.Framework.ReferentialPointers;

public class ReferentialPointerContainer
{
    public ReferentialPointerContainer()
    {
        DefinedItems = new List<ReferentialStateRecord>();
        UsedItems = new List<ReferentialStateRecord>();
        UnusedItems = new List<ReferentialStateRecord>();
        RemovedItems = new List<ReferentialStateRecord>();
    }

    public List<ReferentialStateRecord> DefinedItems { get; set; }
    public List<ReferentialStateRecord> UsedItems { get; set; }
    public List<ReferentialStateRecord> UnusedItems { get; set; }
    public List<ReferentialStateRecord> RemovedItems { get; set; }
}
