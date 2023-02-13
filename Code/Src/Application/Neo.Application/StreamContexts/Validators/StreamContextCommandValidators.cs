using FluentValidation;
using Neo.Application.Contracts.StreamContexts;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.StreamContexts.Validators;

public class StreamContextCommandValidators :
    ICommandValidator<DefiningStreamContextRequested>,
    ICommandValidator<ModifyingStreamContextRequested>,
    ICommandValidator<RemovingStreamContextRequested>
{

    public void Validate(DefiningStreamContextRequested command)
    {
        new DefineStreamContextCommandValidator().ValidateAndThrow(command);
    }

    public void Validate(ModifyingStreamContextRequested command)
    {
        new ModifyStreamContextCommandValidator().ValidateAndThrow(command);
    }

    public void Validate(RemovingStreamContextRequested command)
    {

    }
}