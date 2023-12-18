using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
namespace humanResourceProject.Application.Services.Abstract.IDepartmantServices
{
    public interface IDepartmentWriteService : IBaseWriteService<Department>
    {
        Task<bool> InsertDepartment(DepartmentDTO model);

        Task<bool> UpdateDepartment(DepartmentDTO model);

        Task DeleteDepartment(Guid id);
    }
}
