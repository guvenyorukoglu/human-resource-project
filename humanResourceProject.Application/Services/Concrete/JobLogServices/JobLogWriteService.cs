using humanResourceProject.Application.Services.Abstract.IJobLogServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;

namespace humanResourceProject.Application.Services.Concrete.JobLogServices
{
    public class JobLogWriteService : BaseWriteService<JobLog>, IJobLogWriteService
    {
        public JobLogWriteService(IBaseWriteRepository<JobLog> writeRepository, IBaseReadRepository<JobLog> readRepository) : base(writeRepository, readRepository)
        {
        }
    }
}
