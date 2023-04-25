using Microsoft.AspNetCore.Mvc;
using Neo.Application.Query.Notifications;

namespace Neo.Gateways.RestApi.Notifications;

[ApiController]
[Route("api/[controller]")]
public class NotificationsQueryController : ControllerBase
{
    private readonly INotificationQueryService _queryService;

    public NotificationsQueryController(INotificationQueryService queryService)
    {
        _queryService = queryService;
    }

    [HttpGet("status/{requestId}")]
    public async Task<IActionResult> Get(string requestId,
           CancellationToken cancellationToken)
    {
        var result = await _queryService
            .GetRequestStatus(requestId, cancellationToken)
            .ConfigureAwait(false);
        return Ok(result);
    }
}
