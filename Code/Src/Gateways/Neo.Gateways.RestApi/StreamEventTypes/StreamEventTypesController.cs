using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Gateways.RestApi.StreamEventTypes;

[ApiController]
[Route("api/[controller]")]
public class StreamEventTypesController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public StreamEventTypesController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        DefiningStreamEventTypeRequested command,
        CancellationToken cancellationToken)
    {
        command.Id = Guid.NewGuid();
        await _publishEndpoint.Publish(command, cancellationToken)
            .ConfigureAwait(false);
        return Accepted(command.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id,
        ModifyingStreamEventTypeRequested command,
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
        var command = new RemovingStreamEventTypeRequested
        {
            Id = id,
            Version = version
        };
        await _publishEndpoint.Publish(command, cancellationToken)
           .ConfigureAwait(false);
        return NoContent();
    }
}