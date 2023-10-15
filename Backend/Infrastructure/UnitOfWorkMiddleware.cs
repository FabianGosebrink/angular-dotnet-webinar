namespace ExpenseTracker.Infrastructure;

public static class UnitOfWorkMiddleware
{
    public static void UseUnitOfWorkMiddleware(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            var dbContext = context.RequestServices.GetRequiredService<AppDbContext>();
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            
            try
            {
                await next(context);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }
}