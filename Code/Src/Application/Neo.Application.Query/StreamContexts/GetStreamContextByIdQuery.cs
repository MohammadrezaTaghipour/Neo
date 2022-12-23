namespace Neo.Application.Query.StreamContexts;

public class GetStreamContextByIdQuery
{
    public Guid Id { get; }

    public GetStreamContextByIdQuery(Guid id)
    {
        Id = id;
    }
}