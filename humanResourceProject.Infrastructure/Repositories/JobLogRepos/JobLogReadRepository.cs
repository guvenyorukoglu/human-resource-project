using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;

namespace humanResourceProject.Infrastructure.Repositories.JobLogRepos
{
    public class JobLogReadRepository : BaseReadRepository<JobLog>
    {
        public JobLogReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
