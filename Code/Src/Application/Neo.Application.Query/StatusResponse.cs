namespace Neo.Application.Query;

public record StatusResponse(bool Completed, string ErrorCode, string ErrorMessage);