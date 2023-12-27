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
        Task<List<LeavePersonnelVM>> GetLeavesByEmployeeId(Guid id);
        Task<List<LeaveVM>> GetLeavesByManagerId(Guid id);
        Task<List<LeaveVM>> GetLeavesByCompanyId(Guid id);
        Task<UpdateLeaveDTO> GetUpdateLeaveDTO(Guid id);
        Task<DashboardLeaveVM> FillDashboardLeaveVM(Guid id); 
        Task<LeaveDTO> GetLeaveDTO(Guid employeeId);

    }
}
