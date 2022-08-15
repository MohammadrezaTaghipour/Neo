using Neo.Infrastructure.Framework.Application;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Domain.Contracts.StreamEventTypes;
using System.Threading.Tasks;

namespace Neo.Application.StreamEventTypes;

public class StreamEventTypeService :
    ICommandHandler<DefineStreamEventTypeCommand>,
    ICommandHandler<ModifyStreamEventTypeCommand>,
    ICommandHandler<RemoveStreamEventTypeCommand>
{

    private readonly IStreamEventTypeRepository _repository;
    private readonly IStreamEventTypeArgFactory _argFactory;

    public StreamEventTypeService(
        IStreamEventTypeRepository repository,
        IStreamEventTypeArgFactory argFactory)
    {
        _repository = repository;
        _argFactory = argFactory;
    }

    public async Task Handle(DefineStreamEventTypeCommand command)
    {
        var arg = _argFactory.CreateFrom(command);
        var streamEventType = await StreamEventType.Create(arg);
        await _repository.Add(streamEventType);
    }

    public async Task Handle(ModifyStreamEventTypeCommand command)
    {
        var arg = _argFactory.CreateFrom(command);
        var streamEventType = await _repository.GetBy(
            arg.Id, command.Version);
        streamEventType.Modify(arg);
        await _repository.Add(streamEventType);
    }

    public async Task Handle(RemoveStreamEventTypeCommand command)
    {
        var id = new StreamEventTypeId(command.Id);
        var streamEventType = await _repository.GetBy(id, command.Version);
        streamEventType.Remove();
        await _repository.Add(streamEventType);
    }
}