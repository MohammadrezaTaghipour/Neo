using Microsoft.AspNetCore.Mvc;
using Neo.Application.Query.LifeStreams;

namespace Neo.Gateways.RestApi.LifeStreams
{
    [ApiController]
    [Route("api/[controller]")]
    public class LifeStreamsQueryController : ControllerBase
    {
        private readonly ILifeStreamQueryService _queryService;

        public LifeStreamsQueryController(ILifeStreamQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _queryService.Get(id, cancellationToken);
            return Ok(result);
        }
    }
}
