

using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamEventTypes;

public class DefineStreamEventTypeCommand : ICommand
{
    public DefineStreamEventTypeCommand()
    {
        Metadata = new List<StreamEventTypeMetadataCommandItem>();
    }

    public Guid CorrelationId { get; set; }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public IReadOnlyList<StreamEventTypeMetadataCommandItem> Metadata { get; set; }
}