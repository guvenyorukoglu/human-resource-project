using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Abstract.IPossessionServices
{
    public interface IPossessionReadService : IBaseReadService<Possession>
    {
        Task<PossessionDTO> GetPossessionById(Guid id);
        Task<List<PossessionVM>> GetAllPossessions();
        Task<List<PossessionVM>> GetPossessionsByCompanyId(Guid id);
        Task<UpdatePossessionDTO> GetUpdatePossessionDTO(Guid id);
    }
}
