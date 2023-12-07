using humanResourceProject.Domain.Entities;
using humanResourceProject.Infrastructure.EntityTypeConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Infrastructure.Context
{
    public class AppDbContext:IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    { 
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<CompanyInformation> CompanyDetails { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AppUserConfig());

            base.OnModelCreating(builder);
        }
    }
}
