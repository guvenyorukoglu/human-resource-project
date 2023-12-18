using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
namespace humanResourceProject.Application.Services.Abstract.IJobServices
{
    public interface IJobWriteService : IBaseWriteService<Job>
    {
        Task<bool> InsertJob(JobDTO model);

        Task<bool> UpdateJob(JobDTO model);

        Task DeleteJob(Guid id);
    }
}
