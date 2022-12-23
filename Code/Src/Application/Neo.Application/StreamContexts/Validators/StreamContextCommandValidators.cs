using FluentValidation;
using Neo.Application.Contracts.StreamContexts;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.StreamContexts.Validators;

public class StreamContextCommandValidators :
    ICommandValidator<DefineStreamContextCommand>,
    ICommandValidator<ModifyStreamContextCommand>,
    ICommandValidator<RemoveStreamContextCommand>
{

    public void Validate(DefineStreamContextCommand command)
    {
        new DefineStreamContextCommandValidator().ValidateAndThrow(command);
    }

    public void Validate(ModifyStreamContextCommand command)
    {
        new ModifyStreamContextCommandValidator().ValidateAndThrow(command);
    }

    public void Validate(RemoveStreamContextCommand command)
    {

    }
}