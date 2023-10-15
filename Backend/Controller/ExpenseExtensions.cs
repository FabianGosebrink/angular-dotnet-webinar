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
        
        group.MapGet("/", async (AppDbContext dbContext) => await dbContext.Expenses.AsNoTracking().ToListAsync())
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
        
        group.MapPost("/create", async (AppDbContext dbContext, CreateExpenseDto createExpenseDto, IHubContext<ExpenseHub> hub) =>
        {
            var expense = new Expense(
                createExpenseDto.Name,
                createExpenseDto.Value,
                createExpenseDto.Categories,
                createExpenseDto.Date);
            
            await dbContext.Expenses.AddAsync(expense);
            
            // Get total expenses for the month and notify the user if it exceeds the budget
            var totalExpense = await dbContext.Expenses
                .Where(e => e.ExpenseDate.Year == createExpenseDto.Date.Year && e.ExpenseDate.Month == createExpenseDto.Date.Month)
                .SumAsync(e => e.Value);
            
            if (totalExpense > 5000)
            {
                await hub.Clients.All.SendAsync("Notification", $"Budget exceeded for {createExpenseDto.Date:MMMM yyyy}");
            }
        });

        group.MapGet("/get-total-expense", async (AppDbContext dbContext) =>
        {
            return await dbContext.Expenses
                .SumAsync(e => e.Value);
        });

        group.MapGet("get-total-expense/{year}/{month}", async (int year, int month, AppDbContext dbContext) =>
        {
            return await dbContext.Expenses
                .Where(e => e.ExpenseDate.Year == year && e.ExpenseDate.Month == month)
                .SumAsync(e => e.Value);
        });

        group.WithOpenApi();
    }

    private record CreateExpenseDto(string Name, decimal Value, string[] Categories, DateOnly Date);
}