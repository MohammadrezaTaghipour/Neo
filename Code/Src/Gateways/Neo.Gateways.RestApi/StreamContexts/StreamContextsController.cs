using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Neo.Application.Contracts.StreamContexts;
using Neo.Application.Contracts.StreamEventTypes;

namespace Neo.Gateways.RestApi.StreamContexts;

[ApiController]
[Route("api/[controller]")]
public class StreamContextsController : ControllerBase
{
    IRequestClient<DefiningStreamContextRequested> _definingClient;
    IRequestClient<ModifyingStreamContextRequested> _modifyingClient;
    IRequestClient<RemovingStreamContextRequested> _removingClient;

    public StreamContextsController(
        IRequestClient<DefiningStreamContextRequested> defineClient,
        IRequestClient<ModifyingStreamContextRequested> modifyClient,
        IRequestClient<RemovingStreamContextRequested> removeClient)
    {
        _definingClient = defineClient;
        _modifyingClient = modifyClient;
        _removingClient = removeClient;
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        DefiningStreamContextRequested command,
        CancellationToken cancellationToken)
    {
        await _definingClient
            .GetResponse<DefiningStreamContextRequested>(command, cancellationToken)
            .ConfigureAwait(false);
        return Accepted(command.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id,
        ModifyingStreamContextRequested command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        await _modifyingClient
            .GetResponse<ModifyingStreamContextRequested>(command, cancellationToken)
            .ConfigureAwait(false);
        return Accepted();
    }

    [HttpDelete("{id:guid}/{version:long}")]
    public async Task<IActionResult> Delete(Guid id, int version,
        CancellationToken cancellationToken)
    {
        var command = new RemovingStreamContextRequested
        {
            Id = id,
            Version = version
        };
        await _removingClient
            .GetResponse<RemovingStreamContextRequested>(command, cancellationToken)
            .ConfigureAwait(false);
        return NoContent();
    }
}
