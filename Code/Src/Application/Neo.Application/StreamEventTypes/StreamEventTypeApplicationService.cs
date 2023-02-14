using Neo.Infrastructure.Framework.Application;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Contracts.ReferentialPointers;
using Neo.Domain.Models.ReferentialPointers;

namespace Neo.Application.StreamEventTypes;

public class StreamEventTypeApplicationService :
    IApplicationService<DefiningStreamEventTypeRequested>,
    IApplicationService<ModifyingStreamEventTypeRequested>,
    IApplicationService<RemovingStreamEventTypeRequested>
{
    private readonly IStreamEventTypeRepository _repository;
    private readonly IStreamEventTypeArgFactory _argFactory;
    private readonly IReferentialPointerRepository _referentialPointerRepository;


    public StreamEventTypeApplicationService(
        IStreamEventTypeRepository repository,
        IStreamEventTypeArgFactory argFactory,
        IReferentialPointerRepository referentialPointerRepository)
    {
        _repository = repository;
        _argFactory = argFactory;
        _referentialPointerRepository = referentialPointerRepository;
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

    public async Task Handle(ModifyingStreamEventTypeRequested command,
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

    public async Task Handle(RemovingStreamEventTypeRequested command,
        CancellationToken cancellationToken)
    {
        var id = new StreamEventTypeId(command.Id);
        var streamEventType = await _repository
           .GetBy(id, cancellationToken)
           .ConfigureAwait(false);
        var refPointer = await _referentialPointerRepository
            .GetById(new ReferentialPointerId(command.Id), cancellationToken)
            .ConfigureAwait(false);
        await streamEventType.Remove(refPointer).ConfigureAwait(false);
        await _repository.Add(id, streamEventType, cancellationToken)
            .ConfigureAwait(false);
    }
}