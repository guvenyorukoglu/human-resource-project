using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace humanResourceProject.Infrastructure.EntityTypeConfig
{
    public class JobLogConfig : IEntityTypeConfiguration<JobLog>
    {
        public void Configure(EntityTypeBuilder<JobLog> builder)
        {
            builder.Property(x => x.JobId).IsRequired();
            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.DateOfStart).IsRequired();
            builder.Property(x => x.DateOfTermination).IsRequired(false);
            builder.Property(x => x.ReasonForTermination).IsRequired(false).HasMaxLength(500);

            builder.HasOne(x => x.AppUser).WithMany(x => x.JobLogs).HasForeignKey(x => x.EmployeeId);
        }
    }
}
