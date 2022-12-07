using Microsoft.AspNetCore.Http;
using Neo.Infrastructure.Framework.Domain;
using Newtonsoft.Json;

namespace Neo.Infrastructure.Framework.AspCore
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private string _bcCode;

        public ExceptionHandlerMiddleware(RequestDelegate next, string bcCode)
        {
            _next = next;
            _bcCode = bcCode;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception exception)
            {
                await HandleException(httpContext, exception);
            }
        }

        private async Task HandleException(HttpContext httpContext, Exception exception)
        {
            switch (exception)
            {
                case BusinessException businessException:
                    await HandleBusinessException(httpContext, businessException);
                    break;
                default:
                    await HandleDefaultException(httpContext);
                    break;
            }
        }

        private async Task HandleBusinessException(HttpContext httpContext,
            BusinessException businessException)
        {
            var error = new ErrorResponse(nameof(businessException),
                $"{_bcCode}_{businessException.ErrorCode}");
            await WriteToResponse(httpContext, error, 400);
        }

        private async Task HandleDefaultException(HttpContext httpContext)
        {
            var error = new ErrorResponse("Unhandled exception", _bcCode);
            await WriteToResponse(httpContext, error);
        }

        private static async Task WriteToResponse(HttpContext httpContext,
            ErrorResponse error, int statusCode = 500)
        {
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(error));
        }
    }
}
