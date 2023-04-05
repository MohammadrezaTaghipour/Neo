namespace Neo.Application.Contracts.StreamEventTypes;

public class RemovingStreamEventTypeRequestExecuted
{
    public Guid Id { get; set; }
    public long OriginalVersion { get; set; }
    public long CurrentVersion { get; set; }

}
