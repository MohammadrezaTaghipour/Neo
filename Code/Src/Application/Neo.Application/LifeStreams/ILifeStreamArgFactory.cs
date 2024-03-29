﻿using FizzWare.NBuilder;
using Neo.Application.Contracts.LifeStreams;
using Neo.Domain.Contracts.LifeStreams;
using Neo.Domain.Contracts.StreamContexts;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Models.LifeStreams;
using Neo.Domain.Models.StreamContexts;
using Neo.Domain.Models.StreamEventTypes;

namespace Neo.Application.LifeStreams;

public interface ILifeStreamArgFactory
{
    LifeStreamArg CreateFrom(DefiningLifeStreamRequested command);
    LifeStreamArg CreateFrom(ModifyingLifeStreamRequested command);
    Task<StreamEventArg> CreateFrom(AppendngStreamEventRequested command,
        CancellationToken cancellationToken);
}

public class LifeStreamArgFactory : ILifeStreamArgFactory
{
    private readonly IStreamEventTypeRepository _streamEventTypeRepository;
    private readonly IStreamContextRepository _streamContextRepository;

    public LifeStreamArgFactory(IStreamEventTypeRepository streamEventTypeRepository,
        IStreamContextRepository streamContextRepository)
    {
        _streamEventTypeRepository = streamEventTypeRepository;
        _streamContextRepository = streamContextRepository;
    }

    public LifeStreamArg CreateFrom(DefiningLifeStreamRequested command)
    {
        return LifeStreamArg.Builder
            .With(_ => _.Id, new LifeStreamId(command.Id))
            .With(_ => _.Title = command.Title)
            .With(_ => _.Description = command.Description)
            .Build();
    }

    public LifeStreamArg CreateFrom(ModifyingLifeStreamRequested command)
    {
        return LifeStreamArg.Builder
            .With(_ => _.Id, new LifeStreamId(command.Id))
            .With(_ => _.Title = command.Title)
            .With(_ => _.Description = command.Description)
            .Build();
    }

    public async Task<StreamEventArg> CreateFrom(AppendngStreamEventRequested command,
        CancellationToken cancellationToken)
    {
        var streamEventType = await _streamEventTypeRepository
                .GetBy(new StreamEventTypeId(command.StreamEventTypeId), cancellationToken)
                    .ConfigureAwait(false);

        return StreamEventArg.Builder
            .With(_ => _.Id, new StreamEventId(DateTime.Now.Ticks))
            .With(_ => _.StreamEventType, streamEventType)
            .With(_ => _.StreamContext, await _streamContextRepository
                .GetBy(new StreamContextId(command.StreamContextId), cancellationToken)
                    .ConfigureAwait(false))
            .With(_ => _.Metadata, command.Metadata
                .Select(m => new LifeStreamEventMetada(m.Key, m.Value)).ToList())
            .With(_ => _.StreamEventTypeMetadata, streamEventType.State.Metadata)
            .Build();
    }
}