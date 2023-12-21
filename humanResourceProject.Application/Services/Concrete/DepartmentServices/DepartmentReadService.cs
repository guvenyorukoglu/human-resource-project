using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IDepartmantServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.Application.Services.Concrete.DeparmentServices
{
    
    public class DepartmentReadService : BaseReadService<Department>, IDepartmentReadService
    {
        private readonly IBaseReadRepository<Department> _departmentReadRepository;
        private readonly IMapper _mapper;

        public DepartmentReadService(IBaseReadRepository<Department> departmentReadRepository, IMapper mapper) : base(departmentReadRepository)
        {
            _departmentReadRepository = departmentReadRepository;
            _mapper = mapper;
        }

        public async Task<List<DepartmentVM>> GetAllDepartments()
        {
            List<DepartmentVM>? departments = await _departmentReadRepository.GetFilteredList(
                               select: x => new DepartmentVM
                               {
                                   Id = x.Id,
                                   DepartmentName = x.DepartmentName,
                                   Description = x.Description
                               },
                               where: x => x.Status != Domain.Enum.Status.Deleted && x.Status != Domain.Enum.Status.Inactive,
                               orderBy: x => x.OrderByDescending(x => x.DepartmentName)
               );
            return departments;
        }

        public async Task<DepartmentDTO> GetDepartmentById(Guid id)
        {
            Department department = await _departmentReadRepository.GetById(id);
            DepartmentDTO departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return departmentDTO;
        }

        public async Task<List<DepartmentVM>> GetDepartmentsByCompanyId(Guid id)
        {
            List<DepartmentVM> departments = await _departmentReadRepository.GetFilteredList(
                               select: x => new DepartmentVM
                               {
                                   Id = x.Id,
                                   DepartmentName = x.DepartmentName,
                                   Description = x.Description
                               },
                               where: x => (x.Status != Domain.Enum.Status.Deleted && x.Status != Domain.Enum.Status.Inactive) && x.CompanyId == id,
                               orderBy: x => x.OrderByDescending(x => x.DepartmentName)
               );
            return departments;
        }

        public async Task<Guid> GetIdByDepartmentName(string departmentName)
        {
            Department department = await _departmentReadRepository.GetDefault(x => x.DepartmentName == departmentName);
            return department.Id;
        }
    }
}
