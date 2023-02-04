using Microsoft.AspNetCore.Mvc;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Gateways.Facade.StreamEventTypes;

namespace Neo.Gateways.RestApi.StreamEventTypes;

[ApiController]
[Route("api/[controller]")]
public class StreamEventTypesController : ControllerBase
{
    private readonly IStreamEventTypesFacade _facade;

    public StreamEventTypesController(IStreamEventTypesFacade facade)
    {
        _facade = facade;
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        DefineStreamEventTypeCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _facade.DefineStreamEventType(command, cancellationToken)
            .ConfigureAwait(false);
        return Created("", response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id,
        ModifyStreamEventTypeCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        await _facade.ModifyDefineStreamEventType(command, cancellationToken)
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
        await _facade.RemoveStreamEventType(command, cancellationToken)
            .ConfigureAwait(false);
        return NoContent();
    }
}