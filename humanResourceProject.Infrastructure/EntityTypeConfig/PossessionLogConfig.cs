using humanResourceProject.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace humanResourceProject.Infrastructure.EntityTypeConfig
{
    public class PossessionLogConfig : IEntityTypeConfiguration<PossessionLog>
    {
        public void Configure(EntityTypeBuilder<PossessionLog> builder)
        {
            builder.Property(x => x.StartDateOfPossession).IsRequired();
            builder.Property(x => x.EndDateOfPossession).IsRequired(false);
            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.PossessionId).IsRequired();
           
            builder.HasOne(x => x.AppUser).WithMany(x => x.PossessionLogs).HasForeignKey(x => x.EmployeeId);
        }
    }
}
