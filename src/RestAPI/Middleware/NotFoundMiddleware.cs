using BL.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace RestAPI.Middleware
{
    public static class NotFoundMiddleware
    {
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
