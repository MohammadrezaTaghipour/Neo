using Suzianna.Core.Screenplay;

namespace Neo.Specs.Framework;

public interface ICommandHandler<in T> where T : ICommand
{
    ITask Handle(T command);
}