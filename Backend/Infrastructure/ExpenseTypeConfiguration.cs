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
    }
}