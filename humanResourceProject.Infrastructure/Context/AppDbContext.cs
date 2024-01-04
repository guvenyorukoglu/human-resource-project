using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.EntityTypeConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace humanResourceProject.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Advance> Advances { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Possession> Possessions { get; set; }
        public DbSet<JobLog> JobLogs { get; set; }
        public DbSet<PossessionLog> PossessionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new BaseEntityConfig<AppUser>())
                   .ApplyConfiguration(new BaseEntityConfig<Company>())
                   .ApplyConfiguration(new BaseEntityConfig<Department>())
                   .ApplyConfiguration(new BaseEntityConfig<Job>())
                   .ApplyConfiguration(new BaseEntityConfig<JobLog>())
                   .ApplyConfiguration(new BaseEntityConfig<Possession>())
                   .ApplyConfiguration(new BaseEntityConfig<PossessionLog>())
                   .ApplyConfiguration(new AppUserConfig())
                   .ApplyConfiguration(new CompanyConfig())
                   .ApplyConfiguration(new DepartmentConfig())
                   .ApplyConfiguration(new JobConfig())
                   .ApplyConfiguration(new AdvanceConfig())
                   .ApplyConfiguration(new ExpenseConfig())
                   .ApplyConfiguration(new LeaveConfig())
                   .ApplyConfiguration(new JobLogConfig())
                   .ApplyConfiguration(new PossessionConfig())
                   .ApplyConfiguration(new PossessionLogConfig());

        }
    }
}
