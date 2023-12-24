using humanResourceProject.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace humanResourceProject.Infrastructure.EntityTypeConfig
{
    public class JobConfig : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.Property(j => j.Title).IsRequired().HasMaxLength(50);
            builder.Property(j => j.Description).IsRequired(false).HasMaxLength(500);
            builder.HasOne(j => j.Company).WithMany(c => c.Jobs).HasForeignKey(j => j.CompanyId);
        }
    }
}
