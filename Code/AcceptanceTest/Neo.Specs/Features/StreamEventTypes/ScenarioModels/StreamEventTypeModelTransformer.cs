using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Neo.Specs.Features.StreamEventTypes.ScenarioModels;

[Binding]
public class StreamEventTypeModelTransformer
{
    private readonly ScenarioContext _context;

    public StreamEventTypeModelTransformer(ScenarioContext context)
    {
        _context = context;
    }

    [StepArgumentTransformation]
    public DefineStreamEventTypeCommand ConvertToDefineCommand(Table table)
    {
        var model = table.CreateInstance<StreamEventTypeModel>();
        return new DefineStreamEventTypeCommand
        {
            Id = default,
            Title = model.Title,
            Metadata = _context.ContainsKey(typeof(IReadOnlyCollection<StreamEventTypeMetadataCommandItem>).FullName)
                ? _context.Get<IReadOnlyCollection<StreamEventTypeMetadataCommandItem>>()
                : new List<StreamEventTypeMetadataCommandItem>()
        };
    }

    [StepArgumentTransformation]
    public IReadOnlyCollection<StreamEventTypeMetadataCommandItem> ConvertToMetadaCommandItem(Table table)
    {
        var models = table.CreateSet<StreamEventTypeMetadataModel>();
        return models.Select(model => new StreamEventTypeMetadataCommandItem
        {
            Title = model.Title
        }).ToList();
    }
}