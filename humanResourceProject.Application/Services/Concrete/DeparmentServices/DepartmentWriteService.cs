using humanResourceProject.Application.Services.Abstract.IDepartmantServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;

namespace humanResourceProject.Application.Services.Concrete.DeparmentServices
{
    public class DepartmentWriteService : BaseWriteService<Department>, IDepartmentWriteService
    {
        public DepartmentWriteService(IBaseWriteRepository<Department> writeRepository, IBaseReadRepository<Department> readRepository) : base(writeRepository, readRepository)
        {
        }

        public Task DeleteDepartment(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertDepartment(DepartmentDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDepartment(DepartmentDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
