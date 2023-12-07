using humanResourceProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Infrastructure.EntityTypeConfig
{
    internal class AppUserConfig: BaseEntityConfig<AppUser>
    {
        
        
        public override void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasOne(x=>x.companyInformation).WithMany(x=>x.Employees).HasForeignKey(x=>x.CompanyId);
          
            base.Configure(builder);
        }
    }
}
