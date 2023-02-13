using Newtonsoft.Json;

namespace Neo.Application.Contracts.ReferentialPointers;

public class ReferentialStateRecord
{
    private ReferentialStateRecord()
    {

    }

    [JsonConstructor]
    public ReferentialStateRecord(Guid id, string referentialType)
    {
        Id = id;
        ReferentialType = referentialType;
    }

    public Guid Id { get; }
    public string ReferentialType { get; }
}
