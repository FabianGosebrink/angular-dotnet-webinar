using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Domain;
using ExpenseTracker.Infrastructure;
using Microsoft.AspNetCore.SignalR;

namespace ExpenseTracker.Controller;

public static class ExpenseExtensions
{
    public static void RegisterExpenseEndpoints(this IEndpointRouteBuilder endpoint)
    {
        var group = endpoint.MapGroup("/api/expenses");
        
        group.MapGet("/", async (AppDbContext dbContext, int? page, int? pageSize) =>
            {
                page ??= 0;
                pageSize ??= int.MaxValue;
                return await dbContext
                    .Expenses
                    .OrderBy(b => b.Id)
                    .Skip(page.Value)
                    .Take(pageSize.Value)
                    .AsNoTracking()
                    .ToListAsync();
            })
            .WithDescription("Retrieves all expenses.");
        
        group.MapGet("/{category}", async (string category, AppDbContext dbContext) =>
        {
            return await dbContext.Expenses
                .AsNoTracking()
                .Where(e => e.Categories.Contains(category))
                .ToListAsync();
        });

        group.MapGet("/{year}/{month}", async (int year, int month, AppDbContext dbContext) =>
        {
            return await dbContext.Expenses
                .AsNoTracking()
                .Where(e => e.ExpenseDate.Year == year && e.ExpenseDate.Month == month)
                .ToListAsync();
        });
        
        group.MapPost("/", async (AppDbContext dbContext, CreateExpenseDto createExpenseDto, IHubContext<ExpenseHub> hub) =>
        {
            var expense = new Expense(
                createExpenseDto.Name,
                createExpenseDto.Value,
                createExpenseDto.Categories,
                createExpenseDto.ExpenseDate);
            
            await dbContext.Expenses.AddAsync(expense);
            await dbContext.SaveChangesAsync();
            await NotifyUpdate(hub);
            await NotifyWhenExpenseLimitReached(dbContext, createExpenseDto.ExpenseDate, hub);
            return expense;
        });

        group.MapGet("/get-total-expense", async (AppDbContext dbContext) =>
        {
            return await dbContext.Expenses
                .SumAsync(e => e.Value);
        });

        group.MapGet("/get-all-months", async (AppDbContext dbContext) =>
        {
            return await dbContext.Expenses
                .GroupBy(s => new {s.ExpenseDate.Year, s.ExpenseDate.Month})
                .Select(s => new { Year = s.Key.Year, Month = s.Key.Month, Sum = s.Sum(e => e.Value) })
                .ToArrayAsync();
        })
        .WithDescription("Retrieves all months with the total expenses.");

        group.MapGet("get-total-expense/{year}/{month}", async (int year, int month, AppDbContext dbContext) =>
        {
            return await dbContext.Expenses
                .Where(e => e.ExpenseDate.Year == year && e.ExpenseDate.Month == month)
                .SumAsync(e => e.Value);
        });
        
        group.MapDelete("/{id}", async (int id, AppDbContext dbContext, IHubContext<ExpenseHub> hub) =>
        {
            var expense = await dbContext.Expenses.FindAsync(id)
                ?? throw new InvalidOperationException($"Can't find expense with id {id}");
            dbContext.Expenses.Remove(expense);
            await dbContext.SaveChangesAsync();
            
            await NotifyUpdate(hub);
        });

        group.MapPut("/{id}", async (int id, UpdateExpenseDto dto, AppDbContext dbContext, IHubContext<ExpenseHub> hub) =>
        {
            var update = new Expense(
                dto.Name,
                dto.Value,
                dto.Categories,
                dto.Date);
            
            var expense = await dbContext.Expenses.FindAsync(id) 
                          ?? throw new InvalidOperationException($"Can't find expense with id {dto.Id}");
            
            expense.Update(update);
            await dbContext.SaveChangesAsync();
            await NotifyUpdate(hub);
            await NotifyWhenExpenseLimitReached(dbContext, dto.Date, hub);
        });

        group.WithOpenApi();
    }

    private static async Task NotifyUpdate(IHubContext<ExpenseHub> hub)
    {
        await hub.Clients.All.SendAsync("update");
    }

    private static async Task NotifyWhenExpenseLimitReached(
        AppDbContext dbContext,
        DateOnly expenseDate,
        IHubContext<ExpenseHub> hub)
    {
        // Get total expenses for the month and notify the user if it exceeds the budget
        var allValues = await dbContext.Expenses
            .Where(e => e.ExpenseDate.Year == expenseDate.Year && e.ExpenseDate.Month == expenseDate.Month)
            .Select(e => e.Value)
            .ToListAsync();
            
        var totalExpense = allValues.Sum();

        if (totalExpense > 5000)
        {
            await hub.Clients.All.SendAsync("Notification", $"Budget exceeded for {expenseDate:MMMM yyyy}");
        }
    }

    private record CreateExpenseDto(string Name, double Value, string[] Categories, DateOnly ExpenseDate);

    private record UpdateExpenseDto(int Id, string Name, double Value, string[] Categories, DateOnly Date);
}