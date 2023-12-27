using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
namespace humanResourceProject.Application.Services.Abstract.IJobServices
{
    public interface IJobReadService : IBaseReadService<Job>
    {
        Task<JobDTO> GetJobById(Guid id);

        Task<List<JobVM>> GetAllJobs();
        Task<List<JobVM>> GetJobsByCompanyId(Guid companyId);

    }
}
