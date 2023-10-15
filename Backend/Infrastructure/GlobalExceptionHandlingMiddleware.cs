using System.Net;

namespace ExpenseTracker.Infrastructure;

/***
 * Browser           Middleware           Service
   |                 |                  |
   |---Request------>|                  |
   |                 |---Request------->|
   |                 |<--Response-------|
   |                 X                  X
   |<--Response------|                  |
   |                 |                  |
   |---Request------>|                  |
   |                 |---Request------->|
   |                 |<--Exception------|
   |<--Error Object--|                  |
 **/
public static class GlobalExceptionHandlingMiddleware
{
    public static void UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new { Message = e.Message });
            }
        });
    }
}