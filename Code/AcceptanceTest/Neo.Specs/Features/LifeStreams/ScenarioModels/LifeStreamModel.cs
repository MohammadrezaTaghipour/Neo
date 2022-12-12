using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo.Specs.Features.LifeStreams.ScenarioModels
{
    public class LifeStreamModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ParentStreamModel> ParentStreams { get; set; }
    }
    public class ParentStreamModel
    {
    }
}
