using FluentValidation;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.StreamEventTypes.Validators;

public class StreamEventTypeCommandValidators :
    ICommandValidator<DefiningStreamEventTypeRequested>,
    ICommandValidator<ModifyStreamEventTypeCommand>,
    ICommandValidator<RemoveStreamEventTypeRequested>
{

    public void Validate(DefiningStreamEventTypeRequested command)
    {
        new DefineStreamEventTypeCommandValidator().ValidateAndThrow(command);
    }

    public void Validate(ModifyStreamEventTypeCommand command)
    {
        new ModifyStreamEventTypeCommandValidator().ValidateAndThrow(command);
    }

    public void Validate(RemoveStreamEventTypeRequested command)
    {
        
    }
}
