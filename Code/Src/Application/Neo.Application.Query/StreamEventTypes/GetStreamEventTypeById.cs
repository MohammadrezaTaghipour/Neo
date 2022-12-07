namespace Neo.Application.Query.StreamEventTypes;

public class GetStreamEventTypeByIdQuery
{
    public Guid Id { get; }

    public GetStreamEventTypeByIdQuery(Guid id)
    {
        Id = id;
    }
}
