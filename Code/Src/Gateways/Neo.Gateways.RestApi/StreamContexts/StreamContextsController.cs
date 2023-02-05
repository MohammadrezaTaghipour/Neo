using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Neo.Application.Contracts.StreamContexts;

namespace Neo.Gateways.RestApi.StreamContexts;

[ApiController]
[Route("api/[controller]")]
public class StreamContextsController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public StreamContextsController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        DefiningStreamContextRequested command,
        CancellationToken cancellationToken)
    {
        command.Id = Guid.NewGuid();
        await _publishEndpoint.Publish(command, cancellationToken)
            .ConfigureAwait(false);
        return Accepted(command.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id,
        ModifyStreamContextCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        await _publishEndpoint.Publish(command, cancellationToken)
            .ConfigureAwait(false);
        return Accepted();
    }

    [HttpDelete("{id:guid}/{version:long}")]
    public async Task<IActionResult> Delete(Guid id, int version,
        CancellationToken cancellationToken)
    {
        var command = new RemoveStreamContextRequested
        {
            Id = id,
            Version = version
        };
        await _publishEndpoint.Publish(command, cancellationToken)
            .ConfigureAwait(false);
        return NoContent();
    }
}
