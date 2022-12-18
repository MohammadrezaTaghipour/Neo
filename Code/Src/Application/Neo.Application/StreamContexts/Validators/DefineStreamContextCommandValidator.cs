﻿using FluentValidation;
using Neo.Application.Contracts.StreamContexts;
using Neo.Domain.Contracts.LifeStreams;
using Neo.Domain.Contracts.StreamContexts;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Application.StreamContexts.Validators;

public class DefineStreamContextCommandValidator :
     AbstractValidator<DefineStreamContextCommand>
{
	public DefineStreamContextCommandValidator()
	{
        RuleFor(x => x.Title).Custom((value, _) =>
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessException(StreamContextErrorCodes.SC_BR_10002);

            if (Constants.InvalidCharacters.Any(_ => value.Contains(_)))
                throw new BusinessException(StreamContextErrorCodes.SC_BR_10003);

            if (value.Length > 128)
                throw new BusinessException(StreamContextErrorCodes.SC_BR_10004);
        });

        RuleFor(x => x.Description).Custom((value, _) =>
        {
            if (value.Length > 256)
                throw new BusinessException(StreamContextErrorCodes.SC_BR_10005);
        });

        RuleFor(x => x.StreamEventTypes).Custom((value, _) =>
        {
            if(value == null || !value.Any())
                throw new BusinessException(StreamContextErrorCodes.SC_BR_10006);

            if(value.Any(_=>_.StreamEventTypeId == Guid.Empty))
                throw new BusinessException(StreamContextErrorCodes.SC_BR_10006);
        });
    }
}
