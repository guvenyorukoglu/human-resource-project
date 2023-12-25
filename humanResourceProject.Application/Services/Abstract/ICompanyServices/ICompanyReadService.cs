using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Abstract.ICompanyServices
{
    public interface ICompanyReadService : IBaseReadService<Company>
    {
        Task<CompanyVM> GetCompanyVM(Guid id);
    }
}
