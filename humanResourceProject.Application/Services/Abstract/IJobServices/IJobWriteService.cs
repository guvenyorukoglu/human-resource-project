using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;
namespace humanResourceProject.Application.Services.Abstract.IJobServices
{
    public interface IJobWriteService : IBaseWriteService<Job>
    {
        Task<IdentityResult> InsertJob(JobDTO model);

        Task<IdentityResult>  UpdateJob(JobDTO model);

        Task<IdentityResult> DeleteJob(Guid id);
    }
}
