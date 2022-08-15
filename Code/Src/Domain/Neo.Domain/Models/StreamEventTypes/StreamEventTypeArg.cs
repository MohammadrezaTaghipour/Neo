using Neo.Domain.Contracts.StreamEventTypes;

namespace Neo.Domain.Models.StreamEventTypes;

public class StreamEventTypeArg
{
    
    public StreamEventTypeId Id { get; set; }
    public string Title { get; set; } 
}