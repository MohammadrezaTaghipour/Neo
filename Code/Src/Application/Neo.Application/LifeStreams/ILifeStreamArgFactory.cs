using FizzWare.NBuilder;
using Neo.Application.Contracts.LifeStreams;
using Neo.Domain.Contracts.LifeStreams;
using Neo.Domain.Models.LifeStreams;

namespace Neo.Application.LifeStreams;

public interface ILifeStreamArgFactory
{
    LifeStreamArg CreateFrom(DefineLifeStreamCommand command);
    LifeStreamArg CreateFrom(ModifyLifeStreamCommand command);
}

public class LifeStreamArgFactory : ILifeStreamArgFactory
{
    public LifeStreamArg CreateFrom(DefineLifeStreamCommand command)
    {
        command.Id = LifeStreamId.New().Value;
        return LifeStreamArg.Builder
            .With(_ => _.Id, new LifeStreamId(command.Id))
            .With(_ => _.Title = command.Title)
            .With(_ => _.Description = command.Description)
            .Build();
    }

    public LifeStreamArg CreateFrom(ModifyLifeStreamCommand command)
    {
        return LifeStreamArg.Builder
            .With(_ => _.Id, new LifeStreamId(command.Id))
            .With(_ => _.Title = command.Title)
            .With(_ => _.Description = command.Description)
            .Build();
    }
}