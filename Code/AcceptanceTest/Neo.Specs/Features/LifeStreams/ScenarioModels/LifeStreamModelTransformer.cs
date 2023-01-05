using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Neo.Specs.Features.LifeStreams.ScenarioModels
{
    [Binding]
    public class LifeStreamModelTransformer
    {
        private readonly ScenarioContext _context;

        public LifeStreamModelTransformer(ScenarioContext context)
        {
            _context = context;
        }

        [StepArgumentTransformation]
        public DefineLifeStreamCommand ConvertoToDefineCommand(Table table)
        {
            var model = table.CreateInstance<LifeStreamModel>();
            return new DefineLifeStreamCommand
            {
                Id = default,
                Title = model.Title,
                Description = model.Description
            };
        }

        [StepArgumentTransformation]
        public ModifyLifeStreamCommand ConvertoToModifyCommand(Table table)
        {
            var model = table.CreateInstance<LifeStreamModel>();
            return new ModifyLifeStreamCommand
            {
                Id = _context.Get<DefineLifeStreamCommand>().Id,
                Title = model.Title,
                Description = model.Description,
                Version = 0
            };
        }
    }
}
