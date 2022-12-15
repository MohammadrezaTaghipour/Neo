using Microsoft.AspNetCore.Mvc;
using Neo.Application.Contracts.StreamContexts;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Gateways.RestApi.StreamContexts;

[ApiController]
[Route("api/[controller]")]
public class StreamContextsController : ControllerBase
{
    private readonly ICommandBus _commandBus;

    public StreamContextsController(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        DefineStreamContextCommand command,
        CancellationToken cancellationToken)
    {
        await _commandBus.Dispatch(command, cancellationToken)
            .ConfigureAwait(false);
        return Ok(command.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id,
        ModifyStreamContextCommand command,
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
        var command = new RemoveStreamContextCommand
        {
            Id = id,
            Version = version
        };
        await _commandBus.Dispatch(command, cancellationToken)
            .ConfigureAwait(false);
        return NoContent();
    }
}
