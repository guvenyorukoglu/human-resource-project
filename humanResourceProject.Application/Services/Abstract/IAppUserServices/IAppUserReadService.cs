using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Abstract.IAppUserServices
{
    public interface IAppUserReadService : IBaseReadService<AppUser>
    {
        Task<SignInResult> Login(LoginDTO model);
        Task Logout();
        Task<List<PersonelVM>> GetEmployeesByCompanyId(Guid companyId);
        Task<List<PersonelVM>> GetEmployeesByDepartmentId(Guid departmentId);
        Task<List<ManagerVM>> GetManagersByDepartmentId(Guid deparmentId);

    }
}
