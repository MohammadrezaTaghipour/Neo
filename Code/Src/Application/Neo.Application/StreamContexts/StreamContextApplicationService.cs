using Neo.Application.Contracts.StreamContexts;
using Neo.Domain.Contracts.ReferentialPointers;
using Neo.Domain.Contracts.StreamContexts;
using Neo.Domain.Models.ReferentialPointers;
using Neo.Domain.Models.StreamContexts;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.StreamContexts;

public class StreamContextApplicationService :
     IApplicationService<DefiningStreamContextRequested>,
     IApplicationService<ModifyingStreamContextRequested>,
     IApplicationService<RemovingStreamContextRequested>
{
    private readonly IStreamContextRepository _repository;
    private readonly IStreamContextArgFactory _argFactory;
    private readonly IReferentialPointerRepository _referentialPointerRepository;

    public StreamContextApplicationService(
        IStreamContextRepository repositry,
        IStreamContextArgFactory argFactory,
        IReferentialPointerRepository referentialPointerRepository)
    {
        _repository = repositry;
        _argFactory = argFactory;
        _referentialPointerRepository = referentialPointerRepository;
    }

    public async Task Handle(DefiningStreamContextRequested command,
        CancellationToken cancellationToken)
    {
        var arg = await _argFactory.CreateFrom(command, cancellationToken);
        var streamContext = await StreamContext
            .Create(arg).ConfigureAwait(false);
        await _repository.Add(arg.Id, streamContext, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Handle(ModifyingStreamContextRequested command,
        CancellationToken cancellationToken)
    {
        var arg = await _argFactory.CreateFrom(command, cancellationToken);
        var streamContext = await _repository
            .GetBy(arg.Id, cancellationToken)
            .ConfigureAwait(false);
        await streamContext.Modify(arg).ConfigureAwait(false);
        await _repository.Add(arg.Id, streamContext, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Handle(RemovingStreamContextRequested command,
        CancellationToken cancellationToken)
    {
        var id = new StreamContextId(command.Id);
        var streamContext = await _repository
           .GetBy(id, cancellationToken)
           .ConfigureAwait(false);
        var refPointer = await _referentialPointerRepository
            .GetById(new ReferentialPointerId(command.Id), cancellationToken)
            .ConfigureAwait(false);
        await streamContext.Remove(refPointer).ConfigureAwait(false);
        await _repository.Add(id, streamContext, cancellationToken)
            .ConfigureAwait(false);
    }
}