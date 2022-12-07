using Microsoft.AspNetCore.Mvc;
using Neo.Application.Query.StreamEventTypes;

namespace Neo.Gateways.RestApi.StreamEventTypes;

[ApiController]
[Route("api/[controller]")]
public class StreamEventTypesQueryController : ControllerBase
{
    private readonly IStreamEventTypeQueryService _queryService;

    public StreamEventTypesQueryController(IStreamEventTypeQueryService queryService)
    {
        _queryService = queryService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await _queryService.Get(id, cancellationToken);
        return Ok(result);
    }
}