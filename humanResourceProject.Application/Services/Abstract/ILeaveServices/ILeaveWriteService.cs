using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
namespace humanResourceProject.Application.Services.Abstract.ILeaveServices
{
    public interface ILeaveWriteService : IBaseWriteService<Leave>
    {
        Task<bool> InsertLeave(LeaveDTO model);

        Task<bool> UpdateLeave(LeaveDTO model);

        Task DeleteLeave(Guid id);
    }
}
