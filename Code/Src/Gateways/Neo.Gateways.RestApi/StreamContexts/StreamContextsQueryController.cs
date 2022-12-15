using Microsoft.AspNetCore.Mvc;
using Neo.Application.Query.StreamContexts;

namespace Neo.Gateways.RestApi.StreamContexts;

[ApiController]
[Route("api/[controller]")]
public class StreamContextsQueryController : ControllerBase
{
    private readonly IStreamContextQueryService _queryService;

    public StreamContextsQueryController(IStreamContextQueryService queryService)
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