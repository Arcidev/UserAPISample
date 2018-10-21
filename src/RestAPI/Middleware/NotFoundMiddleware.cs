using BL.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace RestAPI.Middleware
{
    /// <summary>
    /// Not found middleware
    /// </summary>
    public static class NotFoundMiddleware
    {
        /// <summary>
        /// Setups middleware that will intercept <see cref="HttpStatusCode.NotFound"/> returing JSON message with appropriate status code
        /// </summary>
        /// <param name="app">App builder</param>
        public static void UseNotFoundMiddleware(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = ErrorMessages.NotFound }));
                }
            });
        }
    }
}
