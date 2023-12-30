using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;
namespace humanResourceProject.Application.Services.Abstract.IJobServices
{
    public interface IJobWriteService : IBaseWriteService<Job>
    {
        Task<IdentityResult> InsertJob(JobDTO model);

        Task<IdentityResult>  UpdateJob(UpdateJobDTO model);

        Task<IdentityResult> DeleteJob(Guid id);
    }
}
