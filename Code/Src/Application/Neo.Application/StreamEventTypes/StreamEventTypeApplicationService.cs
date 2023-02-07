using Neo.Infrastructure.Framework.Application;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Domain.Contracts.StreamEventTypes;

namespace Neo.Application.StreamEventTypes;

public class StreamEventTypeApplicationService :
    IApplicationService<DefiningStreamEventTypeRequested>,
    IApplicationService<ModifyStreamEventTypeCommand>,
    IApplicationService<RemoveStreamEventTypeRequested>
{
    private readonly IStreamEventTypeRepository _repository;
    private readonly IStreamEventTypeArgFactory _argFactory;

    public StreamEventTypeApplicationService(
        IStreamEventTypeRepository repository,
        IStreamEventTypeArgFactory argFactory)
    {
        _repository = repository;
        _argFactory = argFactory;
    }

    public async Task Handle(DefiningStreamEventTypeRequested command,
        CancellationToken cancellationToken)
    {
        var arg = _argFactory.CreateFrom(command);
        var streamEventType = await StreamEventType
            .Create(arg).ConfigureAwait(false);
        await _repository.Add(arg.Id, streamEventType, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Handle(ModifyStreamEventTypeCommand command,
        CancellationToken cancellationToken)
    {
        var arg = _argFactory.CreateFrom(command);
        var streamEventType = await _repository.GetBy(
                arg.Id, cancellationToken)
            .ConfigureAwait(false);
        await streamEventType.Modify(arg).ConfigureAwait(false);
        await _repository.Add(arg.Id, streamEventType, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Handle(RemoveStreamEventTypeRequested command,
        CancellationToken cancellationToken)
    {
        var id = new StreamEventTypeId(command.Id);
        var streamEventType = await _repository
           .GetBy(id, cancellationToken)
           .ConfigureAwait(false);
        await streamEventType.Remove().ConfigureAwait(false);
        await _repository.Add(id, streamEventType, cancellationToken)
            .ConfigureAwait(false);
    }
}