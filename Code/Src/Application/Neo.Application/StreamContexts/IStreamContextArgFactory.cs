using FizzWare.NBuilder;
using Neo.Application.Contracts.StreamContexts;
using Neo.Domain.Contracts.StreamContexts;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamContexts;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Application.StreamContexts;

public interface IStreamContextArgFactory : IDomainArgFactory
{
    Task<StreamContextArg> CreateFrom(DefiningStreamContextRequested command, CancellationToken cancellationToken);
    Task<StreamContextArg> CreateFrom(ModifyStreamContextCommand command, CancellationToken cancellationToken);
}

public class StreamContextArgFactory : IStreamContextArgFactory
{
    private readonly IStreamEventTypeRepository _streamEventTypeRepository;

    public StreamContextArgFactory(IStreamEventTypeRepository streamEventTypeRepository)
    {
        _streamEventTypeRepository = streamEventTypeRepository;
    }

    public async Task<StreamContextArg> CreateFrom(DefiningStreamContextRequested command,
        CancellationToken cancellationToken)
    {
        command.Id = StreamContextId.New().Value;
        return StreamContextArg.Builder
            .With(_ => _.Id, new StreamContextId(command.Id))
            .With(_ => _.Title, command.Title)
            .With(_ => _.Description, command.Description)
            .With(_ => _.StreamEventTypes, await GetStreamEventTypes(command.StreamEventTypes,
                _streamEventTypeRepository, cancellationToken))
            .Build();
    }

    public async Task<StreamContextArg> CreateFrom(ModifyStreamContextCommand command,
        CancellationToken cancellationToken)
    {
        return StreamContextArg.Builder
            .With(_ => _.Id, new StreamContextId(command.Id))
            .With(_ => _.Title, command.Title)
            .With(_ => _.Description, command.Description)
            .With(_ => _.StreamEventTypes, await GetStreamEventTypes(command.StreamEventTypes,
                _streamEventTypeRepository, cancellationToken))
            .Build();
    }

    static async Task<IReadOnlyCollection<IStreamEventType>> GetStreamEventTypes(
        IReadOnlyCollection<StreamEventTypeCommandItem> streamEventTypes,
        IStreamEventTypeRepository _streamEventTypeRepository,
        CancellationToken cancellationToken)
    {
        var streamEventTypeTasks = streamEventTypes.Select(async _ =>
        {
            return Guid.Empty == _.StreamEventTypeId
                ? null
                : await _streamEventTypeRepository
                .GetBy(new StreamEventTypeId(_.StreamEventTypeId), cancellationToken);
        });
        return await Task.WhenAll(streamEventTypeTasks);
    }
}