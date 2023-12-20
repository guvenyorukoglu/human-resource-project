using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;
namespace humanResourceProject.Application.Services.Abstract.IDepartmantServices
{
    public interface IDepartmentWriteService : IBaseWriteService<Department>
    {
        Task<IdentityResult> InsertDepartment(DepartmentDTO model);

        Task<IdentityResult> UpdateDepartment(UpdateDepartmentDTO model);

        Task<IdentityResult> DeleteDepartment(Guid id);
    }
}
