using Microsoft.AspNetCore.Http.Extensions;

namespace ExpenseTracker.Middleware;

public static class MiddlewareExtensions
{
    public static void UseCorrelationId(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            var correlationId = Guid.NewGuid().ToString();
            context.Response.Headers.Append("Correlation-Id", correlationId);
            await next();
            Console.WriteLine("After Controller");
        });
    }

    public static void UseDisplayUrlConsole(this WebApplication app)
    {
        app.Use((context, next) =>
        {
            var request = context.Request.GetDisplayUrl();
            Console.WriteLine(request);
            return next();
        });
    }
}