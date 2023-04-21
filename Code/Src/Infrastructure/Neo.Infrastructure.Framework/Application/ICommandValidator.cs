namespace Neo.Infrastructure.Framework.Application;

public interface ICommandValidator<in T>
{
    void Validate(T command);
}

