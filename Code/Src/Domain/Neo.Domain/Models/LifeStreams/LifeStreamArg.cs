using FizzWare.NBuilder;
using Neo.Domain.Contracts.LifeStreams;

namespace Neo.Domain.Models.LifeStreams;

public class LifeStreamArg
{
    private LifeStreamArg()
    {
    }

    public LifeStreamId Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }


    public static ISingleObjectBuilder<LifeStreamArg> Builder
        => new Builder().CreateNew<LifeStreamArg>();
}