using MassTransit;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Gateways.Facade.StreamEventTypes;

public class StreamEventTypesConsumers :
    IConsumer<DefineStreamEventTypeCommand>,
    IConsumer<ModifyStreamEventTypeCommand>,
    IConsumer<RemoveStreamEventTypeCommand>
{
    private readonly ICommandBus _commandBus;

    public StreamEventTypesConsumers(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public async Task Consume(
        ConsumeContext<DefineStreamEventTypeCommand> context)
    {
        await ConsumeCommand(context, new CancellationToken())
            .ConfigureAwait(false);
    }

    public async Task Consume(
        ConsumeContext<ModifyStreamEventTypeCommand> context)
    {
        await ConsumeCommand(context, new CancellationToken())
            .ConfigureAwait(false);
    }

    public async Task Consume(
        ConsumeContext<RemoveStreamEventTypeCommand> context)
    {
        await ConsumeCommand(context, new CancellationToken())
            .ConfigureAwait(false);
    }

    private async Task ConsumeCommand<T>(
        ConsumeContext<T> context,
        CancellationToken cancellationToken) where T : class, ICommand
    {
        var command = context.Message;
        try
        {
            await _commandBus.Dispatch(command, cancellationToken)
                .ConfigureAwait(false);
            await context.RespondAsync(command)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await context.RespondAsync(new CommandFalut
            {
                CorrelationId = command.CorrelationId,
                ErrorMessage = ex.Message,
                ErrorCode = (ex as BusinessException)?.ErrorCode,
            }).ConfigureAwait(false);
        }
    }
}
