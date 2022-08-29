using FizzWare.NBuilder;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamEventTypes;
using TestStack.BDDfy;
using Xunit;

namespace Neo.Domain.Tests.Unit.StreamEventTypes;

public class When_defining_stream_event_type
{
    private readonly StreamEventTypeSteps _ = new();

    [Theory]
    [InlineData("Init")]
    [InlineData("Feeling")]
    [InlineData("Guess")]
    [InlineData("Assumption")]
    [InlineData("Conversation")]
    [InlineData("Conclusion")]
    public void Stream_event_type_gets_defined_with_its_valid_properties(string title)
    {
        var properties = StreamEventTypeArg.Builder
            .With(a => a.Id, StreamEventTypeId.New())
            .With(a => a.Title, title)
            .Build();

        this.Given(__ => _.IDefineANewStreamEventTypeWithFollowingProperties(properties))
            .Then(__ => _.ICanFindStreamEventTypeWithAboveProperties(properties))
            .BDDfy();
    }
}