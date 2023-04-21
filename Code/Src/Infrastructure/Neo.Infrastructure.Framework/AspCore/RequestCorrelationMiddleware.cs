using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Neo.Infrastructure.Framework.AspCore;

public class RequestCorrelationMiddleware
{
    readonly RequestDelegate _next;
    public const string CorrelationHeaderKey = "X-ReqId";

    public RequestCorrelationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var id = Guid.NewGuid();
        context.Request.Headers.Add(CorrelationHeaderKey, id.ToString());
        context.Response.Headers.Add(CorrelationHeaderKey, id.ToString());

        await _next(context).ConfigureAwait(false);
    }
}

public static class RequestCorrelationExtensions
{
    public static void UseRequestCorrelationMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<RequestCorrelationMiddleware>();
    }
}
