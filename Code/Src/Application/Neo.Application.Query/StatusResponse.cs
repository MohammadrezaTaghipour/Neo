namespace Neo.Application.Query;

public record StatusResponse(bool Completed, string ErrorCode, string ErrorMessage);


public class RequestStatusResponse
{
    public string Id { get; set; }
    public Guid? EntityId { get; set; }
    public bool Completed { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}
