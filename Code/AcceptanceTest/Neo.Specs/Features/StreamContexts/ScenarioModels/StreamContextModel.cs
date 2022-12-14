
namespace Neo.Specs.Features.StreamContexts.ScenarioModels;

public class StreamContextModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<StreamContextStreamEventTypeModel> StreamEventTypes { get; set; }
}


public class StreamContextStreamEventTypeModel
{
    public string StreamEventType { get; set; }
}