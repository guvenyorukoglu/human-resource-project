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
                               where: x => x.Status != Domain.Enum.Status.Deleted,
                               orderBy: x => x.OrderByDescending(x => x.CreateDate)
               );
            return departments;
        }

        public async Task<DepartmentDTO> GetDepartmentById(Guid id)
        {
            Department department = await _departmentReadRepository.GetById(id);
            DepartmentDTO departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return departmentDTO;
        }
    }
}
