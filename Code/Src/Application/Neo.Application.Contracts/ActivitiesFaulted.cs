﻿
namespace Neo.Application.Contracts;

public class ActivitiesFaulted
{
    public Guid Id { get; set; }
    public string RequestId { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}
