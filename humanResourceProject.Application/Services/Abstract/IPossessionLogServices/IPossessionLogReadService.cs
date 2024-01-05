using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Abstract.IPossessionLogServices
{
    public interface IPossessionLogReadService : IBaseReadService<PossessionLog>
    {
        //Task<PossessionDetailsVM> GetPossessionDetails(Guid id);
        Task<List<PersonelPossessionVM>> GetPossessionsByEmployeeId(Guid employeeId);
    }
}
