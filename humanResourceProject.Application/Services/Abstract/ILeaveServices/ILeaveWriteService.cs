using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;
namespace humanResourceProject.Application.Services.Abstract.ILeaveServices
{
    public interface ILeaveWriteService : IBaseWriteService<Leave>
    {
        Task<IdentityResult> InsertLeave(LeaveDTO model);

        Task<IdentityResult> UpdateLeave(LeaveDTO model);

        Task<IdentityResult> DeleteLeave(Guid id);
    }
}
