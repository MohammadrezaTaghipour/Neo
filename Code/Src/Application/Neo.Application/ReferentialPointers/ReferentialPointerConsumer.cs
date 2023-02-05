using MassTransit;
using Neo.Infrastructure.Framework.ReferentialPointers;

namespace Neo.Application.ReferentialPointers;

public class ReferentialPointerConsumer :
        IConsumer<ReferentialPointerDefined>,
        IConsumer<ReferentialPointerMarkedAsUsed>,
        IConsumer<ReferentialPointerMarkedAsUnused>,
        IConsumer<ReferentialPointerRemoved>
{
    private readonly IReferentialPointerRepository _repository;

    public ReferentialPointerConsumer(IReferentialPointerRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<ReferentialPointerDefined> context)
    {
        var ctoken = new CancellationToken();

        var message = context.Message;
        //if (await _repository.GetById(message.Id, ctoken)
        //    .ConfigureAwait(false) != null)
        //    return;

        var arg = new ReferentialPointerArg
        {
            Id = message.Id,
            PointerType = message.PointerType,
        };
        var refPointer = ReferentialPointer.Create(arg);
        await _repository.Add(message.Id, refPointer, ctoken)
            .ConfigureAwait(false);
    }

    public async Task Consume(ConsumeContext<ReferentialPointerMarkedAsUsed> context)
    {
        var ctoken = new CancellationToken();

        var message = context.Message;
        var refPointer = await _repository.GetById(message.Id, ctoken)
            .ConfigureAwait(false);
        if (!IsPointerAlreadyReferenced(refPointer, message.Id))
        {
            var arg = new ReferentialPointerArg
            {
                PointerType = message.PointerType
            };
            refPointer.MarkAsUsed(arg);
            await _repository.Add(message.Id, refPointer, ctoken)
                .ConfigureAwait(false);
        }
    }

    public async Task Consume(ConsumeContext<ReferentialPointerMarkedAsUnused> context)
    {
        var ctoken = new CancellationToken();

        var message = context.Message;
        var refPointer = await _repository.GetById(message.Id, ctoken)
            .ConfigureAwait(false);
        if (!IsPointerAlreadyReferenced(refPointer, message.Id))
        {
            var arg = new ReferentialPointerArg
            {
                PointerType = message.PointerType
            };
            refPointer.MarkAsUnused(arg);
            await _repository.Add(message.Id, refPointer, ctoken)
                .ConfigureAwait(false);
        }
    }

    public async Task Consume(ConsumeContext<ReferentialPointerRemoved> context)
    {
        var ctoken = new CancellationToken();
        var message = context.Message;
        var refPointer = await _repository.GetById(message.Id, ctoken)
            .ConfigureAwait(false);
        if (!IsPointerAlreadyReferenced(refPointer, message.Id))
        {
            var arg = new ReferentialPointerArg
            {
                PointerType = message.PointerType
            };
            refPointer.Remove(arg);
            await _repository.Add(message.Id, refPointer, ctoken)
                .ConfigureAwait(false);
        }
    }

    static bool IsPointerAlreadyReferenced(ReferentialPointer refPointer,
         ReferentialPointerId newReferentialPointerId)
    {
        return refPointer.IsPointerAlreadyReferenced(newReferentialPointerId);
    }
}