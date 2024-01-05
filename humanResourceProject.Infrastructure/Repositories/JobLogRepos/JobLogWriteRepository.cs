using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;

namespace humanResourceProject.Infrastructure.Repositories.JobLogRepos
{
    public class JobLogWriteRepository : BaseWriteRepository<JobLog>
    {
        public JobLogWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
