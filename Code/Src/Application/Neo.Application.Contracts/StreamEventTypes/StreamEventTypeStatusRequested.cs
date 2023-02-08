using System;

namespace Neo.Application.Contracts.StreamEventTypes;

public class StreamEventTypeStatusRequested
{
    public Guid Id { get; set; }
}

public class StreamEventTypeStatusRequestExecuted
{
    public Guid? Id { get; set; }
    public bool Completed { get; set; }
    public bool Faulted => !string.IsNullOrEmpty(ErrorCode) || !string.IsNullOrEmpty(ErrorMessage);
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}