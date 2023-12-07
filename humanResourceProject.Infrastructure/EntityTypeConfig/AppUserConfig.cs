using humanResourceProject.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Infrastructure.EntityTypeConfig
{
    public class AppUserConfig: BaseEntityConfig<AppUser>
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
            builder.Property(u => u.Title).IsRequired();
            builder.Property(u => u.Job).IsRequired().HasMaxLength(50);
            builder.Property(u => u.ImagePath).IsRequired(false);
                


            builder.HasOne(u=>u.Company).WithMany(c=>c.Employees).HasForeignKey(u=>u.CompanyId);
          
            base.Configure(builder);
        }
    }
}
