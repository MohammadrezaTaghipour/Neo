using FluentValidation;
using Neo.Application.Contracts.LifeStreams;

namespace Neo.Application.LifeStreams.Validators;

public class ModifyLifeStreamCommandValidator :
           AbstractValidator<ModifyLifeStreamCommand>
{
    public ModifyLifeStreamCommandValidator()
    {

    }
}