
namespace Neo.Application.Contracts.StreamEventTypes;

public class StreamEventTypeStatusRequestExecuted
{
    public Guid? Id { get; set; }
    public bool Completed { get; set; }
    public string? ErrorMessage { get; set; }
    //TODO: remove them as soon as you refactored
    public long OriginalVersion { get; set; }
    public long CurrentVersion { get; set; }
}
