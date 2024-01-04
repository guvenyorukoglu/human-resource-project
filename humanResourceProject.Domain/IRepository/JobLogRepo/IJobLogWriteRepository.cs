using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;

namespace humanResourceProject.Domain.IRepository.JobLogRepo
{
    public interface IJobLogWriteRepository : IBaseWriteRepository<JobLog>
    {
    }
}
