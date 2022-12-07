using FluentValidation;
using Neo.Application.Contracts.LifeStreams;
using Neo.Application.LifeStreams.Validators;

namespace Neo.Application.LifeStreams.Validators;

public class DefineLifeStreamCommandValidator :
           AbstractValidator<DefineLifeStreamCommand>
{
    public DefineLifeStreamCommandValidator()
    {

    }
}
