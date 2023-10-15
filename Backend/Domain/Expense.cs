namespace ExpenseTracker.Domain;

public record Expense
{
    public int Id { get; set; }

    public string Name { get; private set; }

    public decimal Value { get; private set; }

    public string[] Categories { get; private set; } = Array.Empty<string>();

    public DateOnly ExpenseDate { get; private set; }

    public Expense(string name, decimal value, string[] categories, DateOnly expenseDate)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        ArgumentNullException.ThrowIfNull(categories);
        
        Name = name;
        Value = value;
        Categories = categories;
        ExpenseDate = expenseDate;
    }

    public void Update(Expense from)
    {
        Name = from.Name;
        Value = from.Value;
        Categories = from.Categories;
        ExpenseDate = from.ExpenseDate;
    }
}