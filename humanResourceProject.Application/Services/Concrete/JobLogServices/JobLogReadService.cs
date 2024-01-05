using humanResourceProject.Application.Services.Abstract.IJobLogServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;

namespace humanResourceProject.Application.Services.Concrete.JobLogServices
{
    public class JobLogReadService : BaseReadService<JobLog>, IJobLogReadService
    {
        public JobLogReadService(IBaseReadRepository<JobLog> readRepository) : base(readRepository)
        {
        }
    }
}
