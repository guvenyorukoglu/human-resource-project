using humanResourceProject.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace humanResourceProject.Infrastructure.EntityTypeConfig
{
    public class AdvanceConfig : IEntityTypeConfiguration<Advance>
    {
        public void Configure(EntityTypeBuilder<Advance> builder)
        {
            builder.Property(a => a.AmountOfAdvance).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(a => a.Explanation).IsRequired(false);

            builder.HasOne(a => a.Employee).WithMany(e => e.Advances).HasForeignKey(a => a.EmployeeId);
        }
    }
}
