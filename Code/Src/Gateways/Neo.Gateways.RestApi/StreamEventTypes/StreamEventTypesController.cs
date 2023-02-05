using Microsoft.AspNetCore.Mvc;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Gateways.RestApi.StreamEventTypes;

[ApiController]
[Route("api/[controller]")]
public class StreamEventTypesController : ControllerBase
{
    private readonly ICommandBus _bus;

    public StreamEventTypesController(ICommandBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        DefineStreamEventTypeCommand command,
        CancellationToken cancellationToken)
    {
        await _bus.Dispatch(command, cancellationToken)
            .ConfigureAwait(false);
        return Created("", command.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id,
        ModifyStreamEventTypeCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        await _bus.Dispatch(command, cancellationToken)
            .ConfigureAwait(false);
        return NoContent();
    }

    [HttpDelete("{id:guid}/{version:long}")]
    public async Task<IActionResult> Delete(Guid id, int version,
        CancellationToken cancellationToken)
    {
        var command = new RemoveStreamEventTypeCommand
        {
            Id = id,
            Version = version
        };
        await _bus.Dispatch(command, cancellationToken)
            .ConfigureAwait(false);
        return NoContent();
    }
}