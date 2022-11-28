using FluentValidation;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.StreamEventTypes.Validators;

public class StreamEventTypeCommandValidators :
    ICommandValidator<DefineStreamEventTypeCommand>,
    ICommandValidator<ModifyStreamEventTypeCommand>
{

    public void Validate(DefineStreamEventTypeCommand command)
    {
        new DefinePartyGroupCommandValidator().ValidateAndThrow(command);
    }

    public void Validate(ModifyStreamEventTypeCommand command)
    {
        new ModifyPartyGroupCommandValidator().ValidateAndThrow(command);
    }
}
