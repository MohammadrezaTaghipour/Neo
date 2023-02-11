namespace Neo.Application.Contracts.LifeStreams;

public class LifeStreamStatusRequested
{
    public Guid Id { get; set; }
}

public class LifeStreamStatusRequestExecuted
{
    public Guid? Id { get; set; }
    public bool Completed { get; set; }
    public bool Faulted { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}