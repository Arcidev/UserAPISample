using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestAPI.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RestAPI.Middleware
{
    /// <summary>
    /// Error handler middleware
    /// </summary>
    public class ErrorHandler
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        /// <summary>
        /// Creates new instance of error handler
        /// </summary>
        /// <param name="next">Delegate to process request</param>
        /// <param name="logger">Logging interface</param>
        public ErrorHandler(RequestDelegate next, ILogger logger)
        {
            this.next = next;
            this.logger = logger;
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
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            logger.LogError(exception, exception.Message);

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
