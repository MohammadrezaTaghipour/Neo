namespace Neo.Infrastructure.Framework.Application;

public interface IRequest
{
    Guid CorrelationId { get; }
}

public abstract class BaseRequest: IRequest
{
    protected BaseRequest()
    {
        CorrelationId = new Guid();
    }

    public Guid CorrelationId { get; }
}