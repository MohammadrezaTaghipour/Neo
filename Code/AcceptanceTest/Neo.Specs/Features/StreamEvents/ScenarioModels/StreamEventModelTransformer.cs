using Neo.Specs.ScreenPlay.StreamEvents.Commands;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Neo.Specs.Features.StreamEvents.ScenarioModels;

[Binding]
public class StreamEventModelTransformer
{
    private readonly ScenarioContext _context;

    public StreamEventModelTransformer(ScenarioContext context)
    {
        _context = context;
    }

    [StepArgumentTransformation]
    public AppendStreamEventCommand ConvertoToDefineCommand(Table table)
    {
        var model = table.CreateInstance<StreamEventModel>();
        return new AppendStreamEventCommand
        {
            Id = default,
            LifeStreamId = _context.ContainsKey(model.LifeStream)
                ? _context.Get<Guid>(model.LifeStream)
                : Guid.Empty,
            StreamContextId = _context.ContainsKey(model.StreamContext)
                ? _context.Get<Guid>(model.StreamContext)
                : Guid.Empty,
            StreamEventTypeId = _context.ContainsKey(model.StreamEventType)
                ? _context.Get<Guid>(model.StreamEventType)
                : Guid.Empty,
            Metadata = new List<StreamEventMetadaCommandItem>()
        };
    }

    [StepArgumentTransformation]
    public IReadOnlyCollection<StreamEventMetadaCommandItem> ConvertToMetadataCommandItem(Table table)
    {
        var models = table.CreateSet<StreamEventMetadaModel>();
        return models.Select(model => new StreamEventMetadaCommandItem
        {
            Key = model.Key,
            Value = model.Value
        }).ToList();
    }
}