using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;

namespace humanResourceProject.Infrastructure.Repositories.LeaveRepos
{
    public class LeaveReadRepository : BaseReadRepository<Leave>
    {
        public LeaveReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
