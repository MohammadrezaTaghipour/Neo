using FizzWare.NBuilder;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Application.StreamEventTypes;

public interface IStreamEventTypeArgFactory : IDomainArgFactory
{
    StreamEventTypeArg CreateFrom(DefiningStreamEventTypeRequested command);
    StreamEventTypeArg CreateFrom(ModifyingStreamEventTypeRequested command);
}

public class StreamEventTypeArgFactory : IStreamEventTypeArgFactory
{
    public StreamEventTypeArg CreateFrom(DefiningStreamEventTypeRequested command)
    {
        return StreamEventTypeArg.Builder
            .With(_ => _.Id, new StreamEventTypeId(command.Id))
            .With(_ => _.Title = command.Title)
            .With(_ => _.Metadata, command.Metadata
                .Select(a => new StreamEventTypeMetadata(a.Title))?.ToList())
            .Build();
    }

    public StreamEventTypeArg CreateFrom(ModifyingStreamEventTypeRequested command)
    {
        return StreamEventTypeArg.Builder
            .With(a => a.Id, new StreamEventTypeId(command.Id))
            .With(a => a.Title = command.Title)
            .With(_ => _.Metadata, command.Metadata
                .Select(a => new StreamEventTypeMetadata(a.Title))?.ToList())
            .Build();
    }
}