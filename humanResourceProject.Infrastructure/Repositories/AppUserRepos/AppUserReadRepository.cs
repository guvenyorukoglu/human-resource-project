using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;

namespace humanResourceProject.Infrastructure.Repositories.AppUserRepos
{
    public class AppUserReadRepository : BaseReadRepository<AppUser>
    {
        public AppUserReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
