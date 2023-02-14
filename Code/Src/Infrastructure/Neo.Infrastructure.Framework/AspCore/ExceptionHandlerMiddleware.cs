using Microsoft.AspNetCore.Http;
using Neo.Infrastructure.Framework.Domain;
using Newtonsoft.Json;

namespace Neo.Infrastructure.Framework.AspCore
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext,
            IErrorResponseBuilder errorResponseBuilder)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception exception)
            {
                await HandleException(httpContext, exception, errorResponseBuilder);
            }
        }

        private async Task HandleException(HttpContext httpContext,
            Exception exception, IErrorResponseBuilder errorResponseBuilder)
        {
            switch (exception)
            {
                case BusinessException businessException:
                    await HandleBusinessException(httpContext, 
                            businessException, errorResponseBuilder);
                    break;
                default:
                    await HandleDefaultException(httpContext, errorResponseBuilder);
                    break;
            }
        }

        private async Task HandleBusinessException(HttpContext httpContext,
            BusinessException businessException,
            IErrorResponseBuilder errorResponseBuilder)
        {
            var error = errorResponseBuilder
                .Buid(businessException.ErrorCode, businessException.Message);
            await WriteToResponse(httpContext, error, 400);
        }

        private async Task HandleDefaultException(HttpContext httpContext,
            IErrorResponseBuilder errorResponseBuilder)
        {
            var error = errorResponseBuilder
                .Buid(string.Empty, "Unhandled exception");
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
