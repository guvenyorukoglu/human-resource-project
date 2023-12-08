using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;

namespace humanResourceProject.Infrastructure.Repositories.AppUserRepos
{
    public class AppUserWriteRepository : BaseWriteRepository<AppUser>
    {
        public AppUserWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
