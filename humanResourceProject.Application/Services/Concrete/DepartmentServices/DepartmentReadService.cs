using humanResourceProject.Application.Services.Abstract.IDepartmantServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Concrete.DeparmentServices
{
    public class DepartmentReadService : BaseReadService<Department>, IDepartmentReadService
    {
        public DepartmentReadService(IBaseReadRepository<Department> readRepository) : base(readRepository)
        {
        }

        public Task<List<DepartmentVM>> GetAllDepartments()
        {
            throw new NotImplementedException();
        }

        public DepartmentDTO GetDepartmentById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
