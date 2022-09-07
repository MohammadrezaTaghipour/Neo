namespace Neo.Specs.Framework;

public interface ICommandBus
{
    void Dispatch<T>(T command) where T : ICommand;
}