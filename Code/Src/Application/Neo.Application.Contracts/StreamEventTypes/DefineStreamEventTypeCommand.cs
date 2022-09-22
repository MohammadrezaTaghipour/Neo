using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamEventTypes;

public class DefineStreamEventTypeCommand : BaseCommand
{
    public DefineStreamEventTypeCommand()
    {
        Metadata = new List<StreamEventTypeMetadataCommandItem>();
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public IReadOnlyCollection<StreamEventTypeMetadataCommandItem> Metadata { get; set; }
}