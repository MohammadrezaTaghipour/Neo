using FluentValidation;
using Neo.Application.Contracts.LifeStreams;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.LifeStreams.Validators;

public class LifeStreamCommandValidators :
    ICommandValidator<DefiningLifeStreamRequested>,
    ICommandValidator<ModifyingLifeStreamRequested>,
    ICommandValidator<RemovingLifeStreamRequested>,
    ICommandValidator<PartialModifyingLifeStreamRequested>
{

    public void Validate(DefiningLifeStreamRequested command)
    {
        new DefineLifeStreamCommandValidator().ValidateAndThrow(command);
    }

    public void Validate(ModifyingLifeStreamRequested command)
    {
        new ModifyLifeStreamCommandValidator().ValidateAndThrow(command);
    }

    public void Validate(RemovingLifeStreamRequested command)
    {

    }

    public void Validate(PartialModifyingLifeStreamRequested command)
    {
        new PartialModifyLifeStreamCommandValidator().ValidateAndThrow(command);
    }
}