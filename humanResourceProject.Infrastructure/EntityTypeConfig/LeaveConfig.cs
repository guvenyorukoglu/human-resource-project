using humanResourceProject.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace humanResourceProject.Infrastructure.EntityTypeConfig
{
    public class LeaveConfig : IEntityTypeConfiguration<Leave>
    {
        public void Configure(EntityTypeBuilder<Leave> builder)
        {
            builder.Property(l => l.StartDateOfLeave).IsRequired().HasColumnType("datetime2");
            builder.Property(l => l.EndDateOfLeave).IsRequired().HasColumnType("datetime2");
            builder.Property(l => l.Explanation).IsRequired(false);
            builder.Property(l => l.DaysOfLeave).IsRequired().HasColumnType("decimal(4,1)");
            builder.Property(a => a.LeaveNo).IsRequired().HasMaxLength(12).HasColumnOrder(2);
            builder.HasOne(l => l.Employee).WithMany(e => e.Leaves).HasForeignKey(l => l.EmployeeId);
        }
    }
}
