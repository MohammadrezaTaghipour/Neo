using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Neo.Application.Contracts.LifeStreams;

namespace Neo.Gateways.RestApi.LifeStreams;

[ApiController]
[Route("api/[controller]")]
public class LifeStreamsController : ControllerBase
{
    IRequestClient<DefiningLifeStreamRequested> _definingClient;
    IRequestClient<ModifyingLifeStreamRequested> _modifyingClient;
    IRequestClient<RemovingLifeStreamRequested> _removingClient;

    public LifeStreamsController(
        IRequestClient<DefiningLifeStreamRequested> defineClient,
        IRequestClient<ModifyingLifeStreamRequested> modifyClient,
        IRequestClient<RemovingLifeStreamRequested> removeClient)
    {
        _definingClient = defineClient;
        _modifyingClient = modifyClient;
        _removingClient = removeClient;
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        DefiningLifeStreamRequested command,
        CancellationToken cancellationToken)
    {
        await _definingClient
           .GetResponse<DefiningLifeStreamRequested>(command, cancellationToken)
           .ConfigureAwait(false);
        return Accepted(command.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id,
        ModifyingLifeStreamRequested command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        await _modifyingClient
            .GetResponse<ModifyingLifeStreamRequested>(command, cancellationToken)
            .ConfigureAwait(false);
        return Accepted();
    }

    [HttpDelete("{id:guid}/{version:long}")]
    public async Task<IActionResult> Delete(Guid id,
        int version, CancellationToken cancellationToken)
    {
        var command = new RemovingLifeStreamRequested
        {
            Id = id,
            Version = version
        };
        await _removingClient
            .GetResponse<RemovingLifeStreamRequested>(command, cancellationToken)
            .ConfigureAwait(false);
        return NoContent();
    }

    //[HttpPatch("{id:guid}")]
    //public async Task<IActionResult> Patch(Guid id,
    //    PartialModifyingLifeStreamRequested command,
    //    CancellationToken cancellationToken)
    //{
    //    command.LifeStreamId = id;
    //    await _commandBus.Dispatch(command, cancellationToken)
    //        .ConfigureAwait(false);
    //    return NoContent();
    //}
}
