namespace Neo.Application.Contracts.StreamContexts;

public class RemovingStreamContextFaulted
{
    public Guid Id { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}