
namespace Neo.Application.Contracts.StreamEventTypes;

public class DefiningStreamEventTypeRequestExecuted
{

    public Guid Id { get; set; }
    public long OriginalVersion { get; set; }
    public long CurrentVersion { get; set; }
}
