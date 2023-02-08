using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamEventTypes;

public class RemovingStreamEventTypeRequested : BaseRequest
{
    public Guid Id { get; set; }
    public long Version { get; set; }
}

public class RemovingStreamEventTypeRequestExecuted
{
    public Guid Id { get; set; }
}


public class RemovingStreamEventTypeFaulted
{
    public Guid Id { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}