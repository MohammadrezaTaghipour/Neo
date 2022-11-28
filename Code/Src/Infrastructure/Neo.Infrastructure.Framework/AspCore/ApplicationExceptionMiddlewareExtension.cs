using Microsoft.AspNetCore.Builder;

namespace Neo.Infrastructure.Framework.AspCore
{
    public static class ApplicationExceptionMiddlewareExtension
    {
        public static void UseApplicationExceptionMiddleware(
            this IApplicationBuilder applicationBuilder, string bcCode)
        {
            applicationBuilder.UseMiddleware(typeof(ExceptionHandlerMiddleware), bcCode);
        }

    }
}
