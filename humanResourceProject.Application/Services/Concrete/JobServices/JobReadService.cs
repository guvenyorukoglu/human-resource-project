using humanResourceProject.Application.Services.Abstract.IJobServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Concrete.JobServices
{
    public class JobReadService : BaseReadService<Job>, IJobReadService
    {
        public JobReadService(IBaseReadRepository<Job> readRepository) : base(readRepository)
        {
        }

        public Task<List<JobVM>> GetAllJobs()
        {
            throw new NotImplementedException();
        }

        public JobDTO GetJobById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
