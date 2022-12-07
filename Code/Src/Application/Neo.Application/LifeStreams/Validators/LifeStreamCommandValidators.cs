using FluentValidation;
using Neo.Application.Contracts.LifeStreams;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.LifeStreams.Validators;

public class LifeStreamCommandValidators :
    ICommandValidator<DefineLifeStreamCommand>,
    ICommandValidator<ModifyLifeStreamCommand>,
    ICommandValidator<RemoveLifeStreamCommand>
{

    public void Validate(DefineLifeStreamCommand command)
    {
        new DefineLifeStreamCommandValidator().ValidateAndThrow(command);
    }

    public void Validate(ModifyLifeStreamCommand command)
    {
        new ModifyLifeStreamCommandValidator().ValidateAndThrow(command);
    }

    public void Validate(RemoveLifeStreamCommand command)
    {

    }
}