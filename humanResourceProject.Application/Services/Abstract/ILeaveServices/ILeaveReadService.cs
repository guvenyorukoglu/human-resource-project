using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
namespace humanResourceProject.Application.Services.Abstract.ILeaveServices
{
    public interface ILeaveReadService : IBaseReadService<Leave>
    {
        Task<LeaveDTO> GetLeaveById(Guid id);

        Task<List<LeaveVM>> GetAllLeaves();
    }
}
