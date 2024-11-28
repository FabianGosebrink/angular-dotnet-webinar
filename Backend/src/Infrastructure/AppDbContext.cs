using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Domain;

namespace ExpenseTracker.Infrastructure;

public sealed class AppDbContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; } = null!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ExpenseTypeConfiguration());
    }
}