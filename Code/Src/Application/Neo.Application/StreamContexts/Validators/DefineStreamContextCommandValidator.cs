using FluentValidation;
using Neo.Application.Contracts.StreamContexts;

namespace Neo.Application.StreamContexts.Validators;

public class DefineStreamContextCommandValidator :
     AbstractValidator<DefineStreamContextCommand>
{
	public DefineStreamContextCommandValidator()
	{

	}
}

public class ModifyStreamContextCommandValidator :
     AbstractValidator<ModifyStreamContextCommand>
{
	public ModifyStreamContextCommandValidator()
	{

	}
}
