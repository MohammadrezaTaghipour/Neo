namespace Neo.Application.Contracts.StreamEventTypes;

public class DefiningStreamEventTypeFaulted
{
    public Guid Id { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}