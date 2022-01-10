namespace DeviceApi.Presentation.Api.Middlewares
{
    using DeviceApi.Infrastructure.CrossCutting.Exceptions;
    using DeviceApi.Infrastructure.CrossCutting.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Middleware responsible to handle Httpstatus codes when exception is thrown
    /// </summary>
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate next;

        private ILogger<ApiExceptionMiddleware> logger;


        /// <summary>
        /// ApiExceptionMiddleware Constructor
        /// </summary>
        public ApiExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// AuthenticationMiddleware method that will be envoked
        /// </summary>
        public async Task InvokeAsync(HttpContext httpContext, IServiceProvider provider)
        {
            this.logger = provider.GetService(typeof(ILogger<ApiExceptionMiddleware>)) as ILogger<ApiExceptionMiddleware>;

            try
            {
                await next(httpContext);
            }
            catch (ApiErrorException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception exceptionObj)
            {
                await HandleExceptionAsync(httpContext, exceptionObj);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, ApiErrorException exception)
        {
            context.Response.ContentType = "application/json";

            var result = new ApiError
            {
                Message = exception.Message,
                ErrorCode = (int)exception.ErrorCode
            };

            context.Response.StatusCode = (int)exception.StatusCode;

            this.logger.Error("Something went wrong", result);

            return context.Response.WriteAsync(result.ToString());
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = new ApiError
            {
                Message = exception.Message,
                ErrorCode = (int)HttpStatusCode.InternalServerError
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            this.logger.Error("Something went wrong", result);

            return context.Response.WriteAsync(result.ToString());
        }
    }
}
