using FluentValidation;
using Neo.Application.Contracts.LifeStreams;
using Neo.Domain.Contracts.LifeStreams;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Application.LifeStreams.Validators;

public class DefineLifeStreamCommandValidator :
    AbstractValidator<DefiningLifeStreamRequested>
{
    public DefineLifeStreamCommandValidator()
    {
        RuleFor(x => x.Id).Custom((value, _) =>
        {
            if (value == Guid.Empty)
                throw new BusinessException(LifeStreamErrorCodes.LS_BR_10001);
        });

        RuleFor(x => x.Title).Custom((value, _) =>
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessException(LifeStreamErrorCodes.LS_BR_10002);

            if (Constants.InvalidCharacters.Any(_ => value.Contains(_)))
                throw new BusinessException(LifeStreamErrorCodes.LS_BR_10003);

            if (value.Length > 128)
                throw new BusinessException(LifeStreamErrorCodes.LS_BR_10004);
        });

        RuleFor(x => x.Description).Custom((value, _) =>
        {
            if (value.Length > 256)
                throw new BusinessException(LifeStreamErrorCodes.LS_BR_10005);
        });
    }
}
