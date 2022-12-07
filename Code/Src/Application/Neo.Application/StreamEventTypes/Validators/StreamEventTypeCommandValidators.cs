using FluentValidation;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.StreamEventTypes.Validators;

public class StreamEventTypeCommandValidators :
    ICommandValidator<DefineStreamEventTypeCommand>,
    ICommandValidator<ModifyStreamEventTypeCommand>,
    ICommandValidator<RemoveStreamEventTypeCommand>
{

    public void Validate(DefineStreamEventTypeCommand command)
    {
        new DefineStreamEventTypeCommandValidator().ValidateAndThrow(command);
    }

    public void Validate(ModifyStreamEventTypeCommand command)
    {
        new ModifyStreamEventTypeCommandValidator().ValidateAndThrow(command);
    }

    public void Validate(RemoveStreamEventTypeCommand command)
    {
        
    }
}
