

namespace Neo.Specs.Features.StreamEvents.ScenarioModels;

public class StreamEventModel
{
    public string LifeStream { get; set; }
    public string StreamContext { get; set; }
    public string StreamEventType { get; set; }
}

public class StreamEventMetadaModel
{
    public string Key { get; set; }
    public string Value { get; set; }
}
