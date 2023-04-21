using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Infrastructure.Framework.AspCore;

public class RequestCorrelationFilterAttribute : IActionFilter
{
    readonly IHttpContextAccessor _httpContextAccessor;

    public RequestCorrelationFilterAttribute(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {

    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var param = context.ActionArguments.SingleOrDefault(p => p.Value is IRequest);
        if (param.Value is IRequest request)
            request.RequestId = _httpContextAccessor.HttpContext
                .Request.Headers[RequestCorrelationMiddleware.CorrelationHeaderKey];
        ;
    }
}