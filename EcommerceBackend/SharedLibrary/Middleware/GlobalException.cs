using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace SharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {

        public async Task InvokeAsync(HttpContext context)
        {
            string message = "Internal server error - from global exception";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";

            try
            {
                await next(context);
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    message = "Rate limit exceeds. Retry after 5 minutes";
                    statusCode = (int)HttpStatusCode.TooManyRequests;
                    title = "Rate limit exceeds";
                    await ModifyResponseHeader(context, title, message, statusCode);
                }

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Not Authenticated";
                    message = "You are not Authenticated";
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    await ModifyResponseHeader(context, title, message, statusCode);
                }
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Unauthorized";
                    message = "You are Unauthorized";
                    statusCode = (int)HttpStatusCode.Forbidden;
                    await ModifyResponseHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);

                if(ex is TaskCanceledException || ex is TimeoutException)
                {
                    message = "Time out";
                    title = "Time out";
                    statusCode = (int)HttpStatusCode.RequestTimeout; //408
                }
                await ModifyResponseHeader(context, title, message, statusCode);

            }

        }

        private static async Task ModifyResponseHeader(HttpContext contex, string title, string message, int statusCode)
        {
            contex.Response.ContentType = "application/json";
            await contex.Response.WriteAsync(JsonSerializer.Serialize (new ProblemDetails()
            {
                Detail = message,
                Status = statusCode,
                Title = title
            }), CancellationToken.None);
            return;
        }
    }
}
