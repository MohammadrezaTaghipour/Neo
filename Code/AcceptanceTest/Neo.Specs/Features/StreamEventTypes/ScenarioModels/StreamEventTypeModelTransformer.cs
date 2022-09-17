using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Neo.Specs.Features.StreamEventTypes.ScenarioModels;

[Binding]
public class StreamEventTypeModelTransformer
{
    [StepArgumentTransformation]
    public DefineStreamEventTypeCommand ConvertToDefineCommand(Table table)
    {
        var model = table.CreateInstance<StreamEventTypeModel>();
        return new DefineStreamEventTypeCommand
        {
            Id = default,
            Title = model.Title,
            Metadata = model.Metadata.Any()
                ? model.Metadata.Select(a => new StreamEventTypeMetadataCommandItem
                {
                    Title = a.Title
                }).ToList()
                : new List<StreamEventTypeMetadataCommandItem>()
        };
    }
}