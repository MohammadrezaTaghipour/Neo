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
        var commands = new DefineStreamEventTypeCommand
        {
            Id = default,
            Title = model.Title,
            Metadata = _context.ContainsKey(typeof(IReadOnlyCollection<StreamEventTypeMetadataCommandItem>).FullName)
                ? _context.Get<IReadOnlyCollection<StreamEventTypeMetadataCommandItem>>()
                : new List<StreamEventTypeMetadataCommandItem>()
        };
        _context.Remove(typeof(IReadOnlyCollection<StreamEventTypeMetadataCommandItem>).FullName);
        return commands;
    }

    [StepArgumentTransformation]
    public IReadOnlyCollection<DefineStreamEventTypeCommand> ConvertToDefineCommands(Table table)
    {
        var models = table.CreateSet<StreamEventTypeModel>();
        return models.Select(model => new DefineStreamEventTypeCommand
        {
            Id = default,
            Title = model.Title,
            Metadata = _context.ContainsKey(typeof(IReadOnlyCollection<StreamEventTypeMetadataCommandItem>).FullName)
                ? _context.Get<IReadOnlyCollection<StreamEventTypeMetadataCommandItem>>()
                : new List<StreamEventTypeMetadataCommandItem>()
        }).ToList();
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

    [StepArgumentTransformation]
    public ModifyStreamEventTypeCommand ConvertToModifyCommand(Table table)
    {
        var model = table.CreateInstance<StreamEventTypeModel>();
        return new ModifyStreamEventTypeCommand
        {
            Id = _context.Get<DefineStreamEventTypeCommand>().Id,
            Title = model.Title,
            Metadata = _context.ContainsKey(typeof(IReadOnlyCollection<StreamEventTypeMetadataCommandItem>).FullName)
                ? _context.Get<IReadOnlyCollection<StreamEventTypeMetadataCommandItem>>()
                : new List<StreamEventTypeMetadataCommandItem>(),
            Version = 0
        };
    }
}
