using Neo.Application.Contracts.LifeStreams;
using Neo.Domain.Contracts.LifeStreams;
using Neo.Domain.Models.LifeStreams;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.LifeStreams;

public class LifeStreamApplicationService :
     IApplicationService<DefineLifeStreamCommand>,
     IApplicationService<ModifyLifeStreamCommand>,
     IApplicationService<RemoveLifeStreamCommand>,
     IApplicationService<PartialModifyLifeStreamCommand>
{
    private readonly ILifeStreamRepository _repository;
    private readonly ILifeStreamArgFactory _argFactory;

    public LifeStreamApplicationService(
        ILifeStreamRepository repositry,
        ILifeStreamArgFactory argFactory)
    {
        _repository = repositry;
        _argFactory = argFactory;
    }

    public async Task Handle(DefineLifeStreamCommand command,
        CancellationToken cancellationToken)
    {
        var arg = _argFactory.CreateFrom(command);
        var lifestream = await LifeStream
            .Create(arg).ConfigureAwait(false);
        await _repository.Add(arg.Id, lifestream, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Handle(ModifyLifeStreamCommand command,
        CancellationToken cancellationToken)
    {
        var arg = _argFactory.CreateFrom(command);
        var lifestream = await _repository.GetBy(arg.Id, cancellationToken)
            .ConfigureAwait(false);
        await lifestream.Modify(arg).ConfigureAwait(false);
        await _repository.Add(arg.Id, lifestream, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Handle(RemoveLifeStreamCommand command,
        CancellationToken cancellationToken)
    {
        var id = new LifeStreamId(command.Id);
        var lifeStream = await _repository
           .GetBy(id, cancellationToken)
           .ConfigureAwait(false);
        await lifeStream.Remove().ConfigureAwait(false);
        await _repository.Add(id, lifeStream, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Handle(PartialModifyLifeStreamCommand command,
        CancellationToken cancellationToken)
    {
        switch (command.OperationType)
        {
            case LifeStreamPartialModificationOperationType.AppendStreamEvent:
                await Handle(new AppendStreamEventCommand
                {
                    LifeStreamId = command.LifeStreamId,
                    StreamContextId = command.StreamContextId,
                    StreamEventTypeId = command.StreamEventTypeId,
                    Metadata = command.Metadata,
                }, cancellationToken).ConfigureAwait(false);
                break;
            case LifeStreamPartialModificationOperationType.RemoveStreamEvent:
                await Handle(new RemoveStreamEventCommand
                {
                    Id = command.Id,
                    LifeStreamId = command.LifeStreamId,
                }, cancellationToken).ConfigureAwait(false);
                break;
            default:
                break;
        }
    }

    public async Task Handle(AppendStreamEventCommand command,
        CancellationToken cancellationToken)
    {
        var id = new LifeStreamId(command.LifeStreamId);
        var lifeStream = await _repository
           .GetBy(id, cancellationToken)
           .ConfigureAwait(false);
        var arg = await _argFactory.CreateFrom(command, cancellationToken)
            .ConfigureAwait(false);
        await lifeStream.AppendStreamEvent(arg).ConfigureAwait(false);
        await _repository.Add(id, lifeStream, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Handle(RemoveStreamEventCommand command,
        CancellationToken cancellationToken)
    {

    }
}