using FluentValidation;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Application.StreamEventTypes;

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

public class DefinePartyGroupCommandValidator :
    AbstractValidator<DefineStreamEventTypeCommand>
{
    public DefinePartyGroupCommandValidator()
    {
        RuleFor(x => x.Title).Custom((value, _) =>
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessException(StreamEventTypeErrorCodes.SET_BR_10002);

            if (Constants.InvalidCharacters.Any(_ => value.Contains(_)))
                throw new BusinessException(StreamEventTypeErrorCodes.SET_BR_10003);

            if (value.Length > 128)
                throw new BusinessException(StreamEventTypeErrorCodes.SET_BR_10004);
        });

        RuleFor(x => x.Metadata).Custom((value, _) =>
        {
            if (value == null || !value.Any()) return;

            if (value.Any(a => string.IsNullOrWhiteSpace(a.Title)))
                throw new BusinessException(StreamEventTypeErrorCodes.SET_BR_10005);

            if (value.Any(a => Constants.InvalidCharacters.Any(__ => a.Title.Contains(__))))
                throw new BusinessException(StreamEventTypeErrorCodes.SET_BR_10006);

            if (value.Any(a => a.Title.Length > 128))
                throw new BusinessException(StreamEventTypeErrorCodes.SET_BR_10007);
        });
    }
}

public class ModifyPartyGroupCommandValidator :
    AbstractValidator<ModifyStreamEventTypeCommand>
{
    public ModifyPartyGroupCommandValidator()
    {
        RuleFor(x => x.Title).Custom((value, _) =>
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessException(StreamEventTypeErrorCodes.SET_BR_10002);

            if (Constants.InvalidCharacters.Any(_ => value.Contains(_)))
                throw new BusinessException(StreamEventTypeErrorCodes.SET_BR_10003);

            if (value.Length > 128)
                throw new BusinessException(StreamEventTypeErrorCodes.SET_BR_10004);
        });

        RuleFor(x => x.Metadata).Custom((value, _) =>
        {
            if (value.Any(a => string.IsNullOrWhiteSpace(a.Title)))
                throw new BusinessException(StreamEventTypeErrorCodes.SET_BR_10005);

            if (value.Any(a => Constants.InvalidCharacters.Any(__ => a.Title.Contains(__))))
                throw new BusinessException(StreamEventTypeErrorCodes.SET_BR_10006);

            if (value.Any(a => a.Title.Length > 128))
                throw new BusinessException(StreamEventTypeErrorCodes.SET_BR_10007);
        });
    }
}