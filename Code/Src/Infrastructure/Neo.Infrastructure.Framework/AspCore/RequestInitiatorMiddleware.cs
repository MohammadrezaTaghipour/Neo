using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Neo.Infrastructure.Framework.Notifications;

namespace Neo.Infrastructure.Framework.AspCore;

public class RequestInitiatorMiddleware
{
    readonly RequestDelegate _next;
    
    public RequestInitiatorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context,
        INotificationPublisher notificationPublisher)
    {
        var id = Guid.NewGuid().ToString();
        context.Request.Headers
            .Add(NeoApplicationConstants.RequestInitiatorHeaderKey, id);
        context.Response.Headers
            .Add(NeoApplicationConstants.RequestInitiatorHeaderKey, id);

        await notificationPublisher
            .Publish(RequestStatusNotificationMessage.Init(id))
            .ConfigureAwait(false);

        await _next(context).ConfigureAwait(false);
    }
}

public static class RequestCorrelationExtensions
{
    public static void UseRequestCorrelationMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<RequestInitiatorMiddleware>();
    }
}
