using FluentAssertions;
using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using Neo.Specs.ScreenPlay.LifeStreams.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.LifeStreams.Then
{
    [Binding]
    public class ICanFindLifeStreamWithAboveProperties
    {
        private readonly ScenarioContext _context;
        private readonly Actor _actor;
        public ICanFindLifeStreamWithAboveProperties(
            ScenarioContext context, Stage stage)
        {
            _context = context;
            _actor = stage.ActorInTheSpotlight;
        }


        [Then("I can find life stream '(.*)' with above properties")]
        public void Func(string title)
        {
            if (_context.Keys.Any(k => k.Contains(nameof(ModifyLifeStreamCommand))))
                AssertModification(title);
            else
                AssertDefinition(title);
        }

        private void AssertDefinition(string expectedTitle)
        {
            var expected = _context.Get<DefineLifeStreamCommand>();
            var actual = _actor.AsksFor(new GetLifeStreamByIdQuestion(expected.Id));

            actual.Should().BeEquivalentTo(expected);
        }

        private void AssertModification(string expectedTitle)
        {
            var id = _context.Get<Guid>(expectedTitle);
            var expected = _context.Get<ModifyLifeStreamCommand>();
            var actual = _actor.AsksFor(new GetLifeStreamByIdQuestion(expected.Id));

            actual.Should().BeEquivalentTo(expected, opt => opt
                .Excluding(_ => _.Version));
        }
    }
}
