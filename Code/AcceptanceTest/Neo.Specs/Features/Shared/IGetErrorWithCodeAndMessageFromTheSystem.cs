using Suzianna.Core.Screenplay.Actors;
using Suzianna.Core.Screenplay;
using Suzianna.Rest.Screenplay.Questions;
using TechTalk.SpecFlow;
using FluentAssertions;

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
            var actual = _actor.AsksFor(LastResponse.Content<ErrorResponse>());
            actual.Code.Should().Be(code);
        }
    }

    public class ErrorResponse
    {
        public string Message { get; }
        public string Code { get; set; }
        public object AdditionalData { get; set; }
    }
}
