using Microsoft.AspNetCore.Mvc;
using Neo.Application.Contracts.LifeStreams;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Gateways.RestApi.LifeStreams;

[ApiController]
[Route("api/[controller]")]
public class LifeStreamsController : ControllerBase
{
    private readonly ICommandBus _commandBus;

    public LifeStreamsController(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        DefineLifeStreamCommand command,
        CancellationToken cancellationToken)
    {
        await _commandBus.Dispatch(command, cancellationToken)
            .ConfigureAwait(false);
        return Ok(command.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id,
        ModifyLifeStreamCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        await _commandBus.Dispatch(command, cancellationToken)
            .ConfigureAwait(false);
        return NoContent();
    }

    [HttpDelete("{id:guid}/{version:long}")]
    public async Task<IActionResult> Delete(Guid id, int version,
        CancellationToken cancellationToken)
    {
        var command = new RemoveLifeStreamCommand
        {
            Id = id,
            Version = version
        };
        await _commandBus.Dispatch(command, cancellationToken)
            .ConfigureAwait(false);
        return NoContent();
    }
}
