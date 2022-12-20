using Neo.Application.Contracts.StreamContexts;
using Neo.Domain.Models.StreamContexts;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.StreamContexts;

public class StreamContextApplicationService :
     IApplicationService<DefineStreamContextCommand>,
     IApplicationService<ModifyStreamContextCommand>,
     IApplicationService<RemoveStreamContextCommand>
{
    private readonly IStreamContextRepository _repository;
    private readonly IStreamContextArgFactory _argFactory;

    public StreamContextApplicationService(
        IStreamContextRepository repositry,
        IStreamContextArgFactory argFactory)
    {
        _repository = repositry;
        _argFactory = argFactory;
    }

    public async Task Handle(DefineStreamContextCommand command,
        CancellationToken cancellationToken)
    {
        var arg = await _argFactory.CreateFrom(command, cancellationToken);
        var streamContext = await StreamContext
            .Create(arg).ConfigureAwait(false);
        await _repository.Add(arg.Id, streamContext, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Handle(ModifyStreamContextCommand command,
        CancellationToken cancellationToken)
    {
        var arg = await _argFactory.CreateFrom(command, cancellationToken);
        var lifestream = await _repository.GetBy(arg.Id, cancellationToken)
            .ConfigureAwait(false);
        await lifestream.Modify(arg).ConfigureAwait(false);
        await _repository.Add(arg.Id, lifestream, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Handle(RemoveStreamContextCommand command,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}