using humanResourceProject.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace humanResourceProject.Infrastructure.EntityTypeConfig
{
    public class ExpenseConfig : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.Property(e => e.AmountOfExpense).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(e => e.DateOfExpense).IsRequired().HasColumnType("datetime2");
            builder.Property(e => e.Explanation).IsRequired(false);
            builder.Property(e => e.FilePath).IsRequired(false);
            builder.Property(e => e.ExpenseNo).IsRequired().HasMaxLength(12).HasColumnOrder(2);
            builder.Property(e => e.RejectReason).IsRequired(false).HasMaxLength(500);
            builder.HasOne(e => e.Employee).WithMany(e => e.Expenses).HasForeignKey(e => e.EmployeeId);
        }
    }
}
