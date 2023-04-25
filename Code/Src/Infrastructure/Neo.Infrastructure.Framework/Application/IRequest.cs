namespace Neo.Infrastructure.Framework.Application;

public interface IRequest
{
    string? RequestId { get; set; }
}

public abstract class BaseRequest : IRequest
{
    public string RequestId { get; set; }
}