using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestAPI.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RestAPI.MiddlewareHandler
{
    /// <summary>
    /// Error handler middleware
    /// </summary>
    public class ErrorHandler
    {
        private readonly RequestDelegate next;

        /// <summary>
        /// Creates new instance of error handler
        /// </summary>
        /// <param name="next">Delegate to process request</param>
        public ErrorHandler(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Executes request and overrides any exception
        /// </summary>
        /// <param name="context">Current http context</param>
        /// <returns>Awaitable task</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Custom exception handler
        /// </summary>
        /// <param name="context">Current http context</param>
        /// <param name="exception">Exception to be handled</param>
        /// <returns>Awaitable task</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            if (exception is UnauthorizedException)
                code = HttpStatusCode.Unauthorized;
            else if (exception is BadRequestException)
                code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new { message = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
