using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
namespace humanResourceProject.Application.Services.Abstract.IAdvanceServices
{
    public interface IAdvanceReadService : IBaseReadService<Advance>
    {
        Task<AdvanceDTO> GetAdvanceById(Guid id);
        Task<List<AdvanceVM>> GetAllAdvances();
        Task<List<AdvancePersonnelVM>> GetAdvancesByEmployeeId(Guid id);
        Task<List<AdvanceVM>> GetAdvancesByDepartmentId(Guid id);
        Task<List<AdvanceVM>> GetAdvancesByCompanyId(Guid id);
        Task<UpdateAdvanceDTO> GetUpdateAdvanceDTO(Guid id);

    }
}
