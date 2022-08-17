using Microsoft.AspNetCore.Mvc;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Gateways.RestApi.StreamEventTypes;

[ApiController]
[Route("api/[controller]")]
public class StreamEventTypeController : ControllerBase
{
    private readonly ICommandBus _commandBus;

    public StreamEventTypeController(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    [HttpPost]
    public async Task<IActionResult> Post(DefineStreamEventTypeCommand command)
    {
        await _commandBus.Dispatch(command).ConfigureAwait(false);
        return Created("", command.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, ModifyStreamEventTypeCommand command)
    {
        command.Id = id;
        await _commandBus.Dispatch(command).ConfigureAwait(false);
        return NoContent();
    }

    [HttpDelete("{id:guid}/{version:long}")]
    public async Task<IActionResult> Delete(Guid id, int version)
    {
        var command = new RemoveStreamEventTypeCommand
        {
            Id = id,
            Version = version
        };
        await _commandBus.Dispatch(command).ConfigureAwait(false);
        return NoContent();
    }
}