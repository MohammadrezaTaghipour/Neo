using FluentAssertions;
using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using Neo.Specs.ScreenPlay.LifeStreams.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Questions;
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
            expected.Id = _actor.AsksFor(LastResponse.Content<Guid>());
            var actual = _actor.AsksFor(new GetLifeStreamByIdQuestion(expected.Id));

            actual.Should().BeEquivalentTo(expected, opt => opt
                .Excluding(_ => _.Id));
        }

        private void AssertModification(string expectedTitle)
        {
            var id = _context.Get<Guid>(expectedTitle);
            var expected = _context.Get<ModifyLifeStreamCommand>();
            var actual = _actor.AsksFor(new GetLifeStreamByIdQuestion(expected.Id));

            actual.Should().BeEquivalentTo(expected, opt => opt
                .Excluding(_ => _.Id)
                .Excluding(_ => _.Version));
        }
    }
}
