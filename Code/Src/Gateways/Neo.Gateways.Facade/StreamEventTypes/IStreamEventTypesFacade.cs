using MassTransit;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Gateways.Facade.StreamEventTypes;

public interface IStreamEventTypesFacade
{
    Task<Guid> DefineStreamEventType(
        DefineStreamEventTypeCommand command,
        CancellationToken cancellationToken);
    Task<Guid> ModifyDefineStreamEventType(
        ModifyStreamEventTypeCommand command,
        CancellationToken cancellationToken);
    Task<Guid> RemoveStreamEventType(
        RemoveStreamEventTypeCommand command,
        CancellationToken cancellationToken);
}

public class StreamEventTypesFacade : IStreamEventTypesFacade
{
    private readonly IRequestClient<DefineStreamEventTypeCommand> _defineRequestClient;
    private readonly IRequestClient<ModifyStreamEventTypeCommand> _modifyRequestClient;
    private readonly IRequestClient<RemoveStreamEventTypeCommand> _removeRequestClient;

    public StreamEventTypesFacade
        (IRequestClient<DefineStreamEventTypeCommand> defineRequestClient,
        IRequestClient<ModifyStreamEventTypeCommand> modifyRequestClient,
        IRequestClient<RemoveStreamEventTypeCommand> removeRequestClient)
    {
        _defineRequestClient = defineRequestClient;
        _modifyRequestClient = modifyRequestClient;
        _removeRequestClient = removeRequestClient;
    }

    public async Task<Guid> DefineStreamEventType(
        DefineStreamEventTypeCommand command,
        CancellationToken cancellationToken)
    {
        var (response, falutResponse) = await _defineRequestClient
               .GetResponse<DefineStreamEventTypeCommand, CommandFalut>(
              command, cancellationToken)
               .ConfigureAwait(false);

        if (response.IsCompletedSuccessfully)
            return (await response).Message.Id;
        else
            return await Fault(falutResponse)
                .ConfigureAwait(false);
    }

    public async Task<Guid> ModifyDefineStreamEventType(
        ModifyStreamEventTypeCommand command,
        CancellationToken cancellationToken)
    {
        var (response, falutResponse) = await _modifyRequestClient
              .GetResponse<ModifyStreamEventTypeCommand, CommandFalut>(
             command, cancellationToken)
              .ConfigureAwait(false);

        if (response.IsCompletedSuccessfully)
            return (await response).Message.Id;
        else
            return await Fault(falutResponse)
                .ConfigureAwait(false);
    }

    public async Task<Guid> RemoveStreamEventType(
        RemoveStreamEventTypeCommand command,
        CancellationToken cancellationToken)
    {
        var (response, falutResponse) = await _removeRequestClient
              .GetResponse<RemoveStreamEventTypeCommand, CommandFalut>(
             command, cancellationToken)
              .ConfigureAwait(false);

        if (response.IsCompletedSuccessfully)
            return (await response).Message.Id;
        else
            return await Fault(falutResponse)
                .ConfigureAwait(false);
    }

    private static async Task<Guid> Fault(
      Task<Response<CommandFalut>> falutResponse)
    {
        var fault = await falutResponse;

        return !string.IsNullOrEmpty(fault.Message.ErrorCode)
            ? throw new BusinessException(fault.Message.ErrorCode)
            : throw new Exception(fault.Message.ErrorMessage);
    }
}