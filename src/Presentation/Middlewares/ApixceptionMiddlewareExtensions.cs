namespace DeviceApi.Presentation.Api.Middlewares
{
    using Microsoft.AspNetCore.Builder;

    public static class ApiExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiExceptions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiExceptionMiddleware>();
        }
    }
}
