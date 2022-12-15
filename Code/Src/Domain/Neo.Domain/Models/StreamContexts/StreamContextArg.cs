using FizzWare.NBuilder;
using Neo.Domain.Contracts.StreamContexts;
using Neo.Domain.Models.StreamEventTypes;

namespace Neo.Domain.Models.StreamContexts;

public class StreamContextArg
{
    private StreamContextArg()
    {
    }

    public StreamContextId Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IReadOnlyCollection<IStreamEventType> StreamEventTypes { get; set; }


    public static ISingleObjectBuilder<StreamContextArg> Builder
        => new Builder().CreateNew<StreamContextArg>();
}