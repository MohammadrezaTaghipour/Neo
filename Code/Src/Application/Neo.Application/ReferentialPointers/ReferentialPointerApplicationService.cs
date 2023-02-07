using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.ReferentialPointers;

namespace Neo.Application.ReferentialPointers;

public class ReferentialPointerApplicationService :
        IApplicationService<ReferentialPointerDefined>,
        IApplicationService<ReferentialPointerMarkedAsUsed>,
        IApplicationService<ReferentialPointerMarkedAsUnused>,
        IApplicationService<ReferentialPointerRemoved>
{
    private readonly IReferentialPointerRepository _repository;

    public ReferentialPointerApplicationService(
        IReferentialPointerRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(ReferentialPointerDefined command, 
        CancellationToken cancellationToken)
    {
        //if (await _repository.GetById(command.Id, cancellationToken)
        //    .ConfigureAwait(false) != null)
        //    return;

        var arg = new ReferentialPointerArg
        {
            Id = command.Id,
            PointerType = command.PointerType,
        };
        var refPointer = ReferentialPointer.Create(arg);
        await _repository.Add(command.Id, refPointer, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Handle(ReferentialPointerMarkedAsUsed command, 
        CancellationToken cancellationToken)
    {
        var refPointer = await _repository.GetById(command.Id, cancellationToken)
            .ConfigureAwait(false);
        if (!IsPointerAlreadyReferenced(refPointer, command.Id))
        {
            var arg = new ReferentialPointerArg
            {
                PointerType = command.PointerType
            };
            refPointer.MarkAsUsed(arg);
            await _repository.Add(command.Id, refPointer, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    public async Task Handle(ReferentialPointerMarkedAsUnused command,
        CancellationToken cancellationToken)
    {
        var refPointer = await _repository.GetById(command.Id, cancellationToken)
            .ConfigureAwait(false);
        if (!IsPointerAlreadyReferenced(refPointer, command.Id))
        {
            var arg = new ReferentialPointerArg
            {
                PointerType = command.PointerType
            };
            refPointer.MarkAsUnused(arg);
            await _repository.Add(command.Id, refPointer, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    public async Task Handle(ReferentialPointerRemoved command, 
        CancellationToken cancellationToken)
    {
        var refPointer = await _repository.GetById(command.Id, cancellationToken)
           .ConfigureAwait(false);
        if (!IsPointerAlreadyReferenced(refPointer, command.Id))
        {
            var arg = new ReferentialPointerArg
            {
                PointerType = command.PointerType
            };
            refPointer.Remove(arg);
            await _repository.Add(command.Id, refPointer, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    static bool IsPointerAlreadyReferenced(ReferentialPointer refPointer,
        ReferentialPointerId newReferentialPointerId)
    {
        return refPointer.IsPointerAlreadyReferenced(newReferentialPointerId);
    }
}
