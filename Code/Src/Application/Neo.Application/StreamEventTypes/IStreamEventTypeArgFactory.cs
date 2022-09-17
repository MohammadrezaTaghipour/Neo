using FizzWare.NBuilder;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Application.StreamEventTypes;

public interface IStreamEventTypeArgFactory : IDomainArgFactory
{
    StreamEventTypeArg CreateFrom(DefineStreamEventTypeCommand command);
    StreamEventTypeArg CreateFrom(ModifyStreamEventTypeCommand command);
}

public class StreamEventTypeArgFactory : IStreamEventTypeArgFactory
{
    public StreamEventTypeArg CreateFrom(DefineStreamEventTypeCommand command)
    {
        command.Id = StreamEventTypeId.New().Value;
        return StreamEventTypeArg.Builder
            .With(a => a.Id, new StreamEventTypeId(command.Id))
            .With(a => a.Title = command.Title)
            .Build();
    }

    public StreamEventTypeArg CreateFrom(ModifyStreamEventTypeCommand command)
    {
        return StreamEventTypeArg.Builder
            .With(a => a.Id, new StreamEventTypeId(command.Id))
            .With(a => a.Title = command.Title)
            .Build();
    }
}