using FluentAssertions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Questions;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.Shared
{
    [Binding]
    public class IGetErrorWithCodeAndMessageFromTheSystem
    {
        private readonly Actor _actor;

        public IGetErrorWithCodeAndMessageFromTheSystem(Stage stage)
        {
            _actor = stage.ActorInTheSpotlight;
        }

        [Then("I get error with code '(.*)' and message '(.*)' from the system")]
        public void Func(string code, string message)
        {
            var actual = _actor.Recall<LastRequestResponse>().Content();
            actual.Code.Should().Be(code);
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public object AdditionalData { get; set; }
    }
}
