using MassTransit;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Infrastructure.Framework.Domain;
using Neo.Infrastructure.Framework.Projections;

namespace Neo.Application.StreamEventTypes.Activities;

public class SyncStreamEventTypeProjectionActivity :
    IActivity<SyncStreamEventTypeProjection, StreamEventTypeActivityLog>
{
    private readonly IDominEventProjectorDispatcher _projectorDispatcher;
    private readonly IStreamEventTypeRepository _repository;
    private readonly IRequestClient<StreamEventTypeStatusRequested> _statusRequestClient;

    public SyncStreamEventTypeProjectionActivity(
        IRequestClient<StreamEventTypeStatusRequested> statusRequestClient,
        IStreamEventTypeRepository repository,
        IDominEventProjectorDispatcher projectorDispatcher)
    {
        _statusRequestClient = statusRequestClient;
        _repository = repository;
        _projectorDispatcher = projectorDispatcher;
    }

    public async Task<ExecutionResult> Execute(
        ExecuteContext<SyncStreamEventTypeProjection> context)
    {
        var request = context.Arguments;

        var status = (await _statusRequestClient
               .GetResponse<StreamEventTypeStatusRequestExecuted>(
               new StreamEventTypeStatusRequested
               {
                   Id = request.Id
               }).ConfigureAwait(false)).Message;

        var streamEventType = await _repository
            .GetBy(new StreamEventTypeId(request.Id), context.CancellationToken);

        List<IDomainEvent> changeEvents = new();
        if (status.OriginalVersion != 0)
        {
            changeEvents = streamEventType.Current
                .Where(_ => _.Version > status.OriginalVersion
                            && _.Version <= status.CurrentVersion)
                .ToList();
        }
        else
        {
            changeEvents = streamEventType.Current
               .Where(_ => _.Version <= status.CurrentVersion)
               .ToList();
        }

        foreach (var eventItem in changeEvents.OrderBy(_ => _.Version))
        {
            await _projectorDispatcher
                .Dispatch((dynamic)eventItem, context.CancellationToken)
                .ConfigureAwait(false);
        }

        return context.Completed();
    }

    public async Task<CompensationResult> Compensate(
        CompensateContext<StreamEventTypeActivityLog> context)
    {
        await Task.CompletedTask;
        return context.Compensated();
    }
}
