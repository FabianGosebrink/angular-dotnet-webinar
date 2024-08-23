using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseTracker.Domain;

namespace ExpenseTracker.Infrastructure;

public class ExpenseTypeConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(1024);
        builder.Property(x => x.Value).IsRequired();
        builder.Property(x => x.Categories)
            .IsRequired()
            .HasMaxLength(1024);
        builder.Property(x => x.ExpenseDate)
            .IsRequired();

        builder.HasData(GenerateSeedData());
    }

    private static IEnumerable<Expense> GenerateSeedData()
    {
        for (var year = 2023; year <= 2024; year++)
        {
            for (var month = 1; month <= 12; month++)
            {
                var entriesPerMonth = Random.Shared.Next(3, 6);

                for (var i = 0; i < entriesPerMonth; i++)
                {
                    var daysInMonth = DateTime.DaysInMonth(year, month);
                    var day = Random.Shared.Next(1, daysInMonth + 1);
                    var expenseDate = new DateOnly(year, month, day);

                    var expense = new Expense(
                        name: GenerateRandomExpenseName(),
                        value: Math.Round(Random.Shared.NextDouble() * 1000 + 1, 2),
                        categories: GenerateRandomCategories(),
                        expenseDate: expenseDate
                    );

                    yield return expense;
                }
            }
        }
    }
    
    private static string GenerateRandomExpenseName()
    {
        string[] expenseNames =
        [
            "Groceries", "Utilities", "Rent", "Entertainment", "Transport", "Healthcare", "Dining Out", "Education",
            "Gifts", "Clothing"
        ];
        
        return expenseNames[Random.Shared.Next(expenseNames.Length)];
    }
    
    private static string[] GenerateRandomCategories()
    {
        string[][] possibleCategories =
        [
            ["Home", "Food"],
            ["Transport", "Fuel"],
            ["Health", "Fitness"],
            ["Entertainment", "Movies"],
            ["Utilities", "Electricity"],
            ["Travel", "Vacation"],
            ["Clothing", "Accessories"],
            ["Dining", "Food"],
            ["Education", "Books"]
        ];
        return possibleCategories[Random.Shared.Next(possibleCategories.Length)];
    }
}