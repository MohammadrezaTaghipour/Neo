namespace Neo.Specs.Features.StreamEventTypes.ScenarioModels;

public class StreamEventTypeModel
{
    public string Title { get; set; }
    public List<StreamEventTypeMetadataModel> Metadata { get; set; }
}

public class StreamEventTypeMetadataModel
{
    public string Title { get; set; }
}