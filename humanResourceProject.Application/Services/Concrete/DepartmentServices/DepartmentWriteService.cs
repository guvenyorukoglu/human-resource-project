using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IDepartmantServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Domain.IRepository.DepartmentRepo;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Concrete.DepartmentServices
{
    public class DepartmentWriteService : BaseWriteService<Department>, IDepartmentWriteService
    {
        private readonly IBaseWriteRepository<Department> _baseWriteRepository;
        private readonly IBaseReadRepository<Department> _baseReadRepository;
        private readonly IDepartmentWriteRepository _departmentWriteRepository;
        private readonly IMapper _mapper;
        public DepartmentWriteService(IBaseWriteRepository<Department> writeRepository, IBaseReadRepository<Department> readRepository, IDepartmentWriteRepository departmentWriteRepository, IMapper mapper) : base(writeRepository, readRepository)
        {
            _baseWriteRepository = writeRepository;
            _baseReadRepository = readRepository;
            _departmentWriteRepository = departmentWriteRepository;
            _mapper = mapper;
        }

        public async Task<IdentityResult> DeleteDepartment(Guid id)
        {
            Department department = await _baseReadRepository.GetById(id);
            department.Status = Domain.Enum.Status.Deleted;
            department.DeleteDate = DateTime.Now;
            if(await _baseWriteRepository.Delete(id))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> InsertDepartment(DepartmentDTO model)
        {
            if (model == null)
                return IdentityResult.Failed();

            Department newDepartment = _mapper.Map<Department>(model);
            newDepartment.Status = Domain.Enum.Status.Active;
            newDepartment.CreateDate = DateTime.Now;
            if (await _baseWriteRepository.Insert(newDepartment))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateDepartment(DepartmentDTO model)
        {
            Department department = await _baseReadRepository.GetSingleDefault(x => x.Id == model.Id);
            if (department == null)
                return IdentityResult.Failed();

            DepartmentDTO departmentDTO = _mapper.Map<DepartmentDTO>(department);

            departmentDTO.DepartmentName = model.DepartmentName;
            departmentDTO.Description = model.Description;
            departmentDTO.Status = Domain.Enum.Status.Modified;
            departmentDTO.UpdateDate = DateTime.Now;

            if (await _baseWriteRepository.Update(_mapper.Map<Department>(departmentDTO)))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }
    }
}
