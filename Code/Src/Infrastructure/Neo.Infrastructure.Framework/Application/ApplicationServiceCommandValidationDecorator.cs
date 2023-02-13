
namespace Neo.Infrastructure.Framework.Application;

public class ApplicationServiceCommandValidationDecorator<T> :
    IApplicationService<T> where T : IRequest
{
    private readonly IApplicationService<T> _applicationService;
    private readonly ICommandValidator<T> _commandValidator;

    public ApplicationServiceCommandValidationDecorator(
        IApplicationService<T> applicationService,
        ICommandValidator<T> commandValidator)
    {
        _applicationService = applicationService;
        _commandValidator = commandValidator;
    }

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        _commandValidator.Validate(command);
        await _applicationService.Handle(command, cancellationToken)
            .ConfigureAwait(false);
    }
}