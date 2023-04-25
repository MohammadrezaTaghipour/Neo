namespace Neo.Application.Contracts.StreamContexts;

public class StreamContextStatusRequested
{
    public Guid Id { get; set; }
}

public class StreamContextStatusRequestExecuted
{
    public Guid? Id { get; set; }
    public bool Completed { get; set; }
    public string? ErrorMessage { get; set; }
    public long OriginalVersion { get; set; }
    public long CurrentVersion { get; set; }
}