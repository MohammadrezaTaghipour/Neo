namespace Neo.Application.Query.LifeStreams;

public class GetLifeStreamByIdQuery
{
    public Guid Id { get; }

    public GetLifeStreamByIdQuery(Guid id)
    {
        Id = id;
    }
}