using Newtonsoft.Json;

namespace Neo.Application.Contracts.ReferentialPointers;

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

    public ReferentialPointerContainer? Clone()
    {
        var serialized = JsonConvert.SerializeObject(this);
        return JsonConvert.DeserializeObject<ReferentialPointerContainer>(serialized);
    }
}
