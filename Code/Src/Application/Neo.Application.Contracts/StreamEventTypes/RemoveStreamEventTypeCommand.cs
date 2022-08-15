using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamEventTypes;

public class RemoveStreamEventTypeCommand : ICommand
{
    public Guid CorrelationId { get; set; }
    public Guid Id { get; set; }
    public int Version { get; set; }
}