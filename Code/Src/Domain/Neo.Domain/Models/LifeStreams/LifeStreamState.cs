using Neo.Domain.Contracts.LifeStreams;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.LifeStreams;

public record LifeStreamState : AggregateState<LifeStreamState>
{
    private List<ParentLifeStream> _parentStream = new();

    public LifeStreamId Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool Deleted { get; private set; }
    public IReadOnlyCollection<ParentLifeStream> PanrentStreams => _parentStream.AsReadOnly();

    public override LifeStreamState When(IDomainEvent eventToHandle)
    {
        return When((dynamic)eventToHandle);
    }

    private LifeStreamState When(LifeStreamDefined eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Title = eventToHandle.Title,
            Description = eventToHandle.Description,
        };
    }

    private LifeStreamState When(LifeStreamModified eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Title = eventToHandle.Title,
            Description = eventToHandle.Description,
        };
    }

    private LifeStreamState When(LifeStreamRemoved eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Deleted = true
        };
    }
}
