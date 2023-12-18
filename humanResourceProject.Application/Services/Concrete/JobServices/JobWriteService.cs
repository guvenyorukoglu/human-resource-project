using humanResourceProject.Application.Services.Abstract.IJobServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;

namespace humanResourceProject.Application.Services.Concrete.JobServices
{
    public class JobWriteService : BaseWriteService<Job>, IJobWriteService
    {
        public JobWriteService(IBaseWriteRepository<Job> writeRepository, IBaseReadRepository<Job> readRepository) : base(writeRepository, readRepository)
        {
        }

        public Task DeleteJob(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertJob(JobDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateJob(JobDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
