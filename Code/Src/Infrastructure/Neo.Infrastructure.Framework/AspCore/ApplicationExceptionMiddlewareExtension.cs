using Microsoft.AspNetCore.Builder;

namespace Neo.Infrastructure.Framework.AspCore
{
    public static class ApplicationExceptionMiddlewareExtension
    {
        public static void UseApplicationExceptionMiddleware(
            this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware(typeof(ExceptionHandlerMiddleware));
        }
    }
}
