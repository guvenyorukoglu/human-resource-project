using humanResourceProject.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace humanResourceProject.Infrastructure.EntityTypeConfig
{
    public class AppUserConfig : BaseEntityConfig<AppUser>
    {
        public override void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(20);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(30);
            builder.Property(u => u.MiddleName).IsRequired(false).HasMaxLength(30);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Address).IsRequired().HasMaxLength(100);
            builder.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(13);
            builder.Property(u => u.IdentificationNumber).IsRequired().HasMaxLength(11);
            builder.Property(u => u.BloodGroup).IsRequired();
            builder.Property(u => u.Birthdate).IsRequired();
            builder.Property(u => u.Gender).IsRequired();
            builder.Property(u => u.ImagePath).IsRequired(false);
            builder.Property(u => u.ManagerId).IsRequired(false);

            builder.HasOne(u => u.Department).WithMany(d => d.Employees).HasForeignKey(u => u.DepartmentId);
            builder.HasOne(u => u.Job).WithMany(j => j.Employees).HasForeignKey(u => u.JobId);
            builder.HasOne(u => u.Manager).WithMany(m => m.DepartmentEmployees).HasForeignKey(u => u.ManagerId).OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
