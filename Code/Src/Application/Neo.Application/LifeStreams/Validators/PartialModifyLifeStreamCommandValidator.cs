using FluentValidation;
using Neo.Application.Contracts.LifeStreams;
using Neo.Domain.Contracts.LifeStreams;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Application.LifeStreams.Validators;

public class PartialModifyLifeStreamCommandValidator :
     AbstractValidator<PartialModifyLifeStreamCommand>
{
    public PartialModifyLifeStreamCommandValidator()
    {
        RuleFor(x => x.LifeStreamId).Custom((value, _) =>
        {
            if (value == Guid.Empty)
                throw new BusinessException(StreamEventErrorCodes.SE_BR_10002);
        });
        
        RuleFor(x => x.StreamContextId).Custom((value, _) =>
        {
            if (value == Guid.Empty)
                throw new BusinessException(StreamEventErrorCodes.SE_BR_10003);
        });
        
        RuleFor(x => x.StreamEventTypeId).Custom((value, _) =>
        {
            if (value == Guid.Empty)
                throw new BusinessException(StreamEventErrorCodes.SE_BR_10004);
        });
        
        RuleFor(x => x.Metadata).Custom((value, _) =>
        {
            if (value.Any(_ => string.IsNullOrEmpty(_.Value)))
                throw new BusinessException(StreamEventErrorCodes.SE_BR_10005);
        });
    }
}
