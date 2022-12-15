using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Neo.Specs.Features.StreamContexts.ScenarioModels;

[Binding]
public class StreamContextModelTransformer
{
    private readonly ScenarioContext _context;

    public StreamContextModelTransformer(ScenarioContext context)
    {
        _context = context;
    }

    [StepArgumentTransformation]
    public DefineStreamContextCommand ConvertToDefineCommand(Table table)
    {
        var model = table.CreateInstance<StreamContextModel>();
        return new DefineStreamContextCommand
        {
            Id = default,
            Title = model.Title,
            Description = model.Description,
            StreamEventTypes = new List<StreamEventTypeCommandItem>()
        };
    }

    [StepArgumentTransformation]
    public ModifyStreamContextCommand ConvertToModifyCommand(Table table)
    {
        var model = table.CreateInstance<StreamContextModel>();
        return new ModifyStreamContextCommand
        {
            Id = _context.Get<DefineStreamContextCommand>().Id,
            Title = model.Title,
            StreamEventTypes = default,
            Version = 0
        };
    }
    
    [StepArgumentTransformation]
    public IReadOnlyCollection<StreamEventTypeCommandItem> ConvertToCommandItems(Table table)
    {
        var models = table.CreateSet<StreamContextStreamEventTypeModel>();
        return models.Select(model =>
        new StreamEventTypeCommandItem {
            StreamEventTypeId = ! string.IsNullOrEmpty(model.StreamEventType)
            ? _context.Get<Guid>(model.StreamEventType)
            : Guid.Empty
        }).ToList();
    }
}