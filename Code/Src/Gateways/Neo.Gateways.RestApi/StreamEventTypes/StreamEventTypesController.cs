using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Neo.Application.Contracts.StreamEventTypes;

namespace Neo.Gateways.RestApi.StreamEventTypes;

[ApiController]
[Route("api/[controller]")]
public class StreamEventTypesController : ControllerBase
{
    IRequestClient<DefiningStreamEventTypeRequested> _definingClient;
    IRequestClient<ModifyingStreamEventTypeRequested> _modifyingClient;
    IRequestClient<RemovingStreamEventTypeRequested> _removingClient;

    public StreamEventTypesController(
        IRequestClient<DefiningStreamEventTypeRequested> defineClient,
        IRequestClient<ModifyingStreamEventTypeRequested> modifyClient,
        IRequestClient<RemovingStreamEventTypeRequested> removeClient)
    {
        _definingClient = defineClient;
        _modifyingClient = modifyClient;
        _removingClient = removeClient;
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        DefiningStreamEventTypeRequested command,
        CancellationToken cancellationToken)
    {
        await _definingClient
            .GetResponse<DefiningStreamEventTypeRequested>(command, cancellationToken)
            .ConfigureAwait(false);
        return Accepted(command.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id,
        ModifyingStreamEventTypeRequested command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        await _modifyingClient
            .GetResponse<ModifyingStreamEventTypeRequested>(command, cancellationToken)
            .ConfigureAwait(false);
        return Accepted();
    }

    [HttpDelete("{id:guid}/{version:long}")]
    public async Task<IActionResult> Delete(Guid id, int version,
        CancellationToken cancellationToken)
    {
        var command = new RemovingStreamEventTypeRequested
        {
            Id = id,
            Version = version
        };
        await _removingClient
            .GetResponse<RemovingStreamEventTypeRequested>(command, cancellationToken)
            .ConfigureAwait(false);
        return NoContent();
    }
}
