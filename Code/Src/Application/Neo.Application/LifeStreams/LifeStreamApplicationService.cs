using Neo.Application.Contracts.LifeStreams;
using Neo.Domain.Contracts.LifeStreams;
using Neo.Domain.Models.LifeStreams;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.LifeStreams;

public class LifeStreamApplicationService :
     IApplicationService<DefiningLifeStreamRequested>,
     IApplicationService<ModifyingLifeStreamRequested>,
     IApplicationService<RemovingLifeStreamRequested>,
     IApplicationService<AppendngStreamEventRequested>,
     IApplicationService<RemovingStreamEventRequested>
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

    public async Task Handle(DefiningLifeStreamRequested command,
        CancellationToken cancellationToken)
    {
        var arg = _argFactory.CreateFrom(command);
        var lifestream = await LifeStream
            .Create(arg).ConfigureAwait(false);
        await _repository.Add(arg.Id, lifestream, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Handle(ModifyingLifeStreamRequested command,
        CancellationToken cancellationToken)
    {
        var arg = _argFactory.CreateFrom(command);
        var lifestream = await _repository.GetBy(arg.Id, cancellationToken)
            .ConfigureAwait(false);
        await lifestream.Modify(arg).ConfigureAwait(false);
        await _repository.Add(arg.Id, lifestream, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Handle(RemovingLifeStreamRequested command,
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

    public async Task Handle(AppendngStreamEventRequested command,
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

    public async Task Handle(RemovingStreamEventRequested command,
        CancellationToken cancellationToken)
    {
        var id = new LifeStreamId(command.LifeStreamId);
        var lifeStream = await _repository
           .GetBy(id, cancellationToken)
           .ConfigureAwait(false);
        await lifeStream.RemoveStreamEvent(new StreamEventId(command.Id))
            .ConfigureAwait(false);
        await _repository.Add(id, lifeStream, cancellationToken)
          .ConfigureAwait(false);
    }
}