using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IDepartmantServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Concrete.DepartmentServices
{
    public class DepartmentWriteService : BaseWriteService<Department>, IDepartmentWriteService
    {
        private readonly IBaseWriteRepository<Department> _departmentWriteRepository;
        private readonly IBaseReadRepository<Department> _departmentReadRepository;
        private readonly IMapper _mapper;
        public DepartmentWriteService(IBaseWriteRepository<Department> departmentWriteRepository, IBaseReadRepository<Department> departmentReadRepository, IMapper mapper) : base(departmentWriteRepository, departmentReadRepository)
        {
            _departmentWriteRepository = departmentWriteRepository;
            _departmentReadRepository = departmentReadRepository;
            _mapper = mapper;
        }

        public async Task<IdentityResult> DeleteDepartment(Guid id)
        {
            Department department = await _departmentReadRepository.GetById(id);
            department.Status = Domain.Enum.Status.Deleted;
            department.DeleteDate = DateTime.Now;
            if (await _departmentWriteRepository.Delete(id))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> InsertDepartment(DepartmentDTO model)
        {
            Department department = await _departmentReadRepository.GetSingleDefault(x => x.DepartmentName == model.DepartmentName && (x.Status == Domain.Enum.Status.Active || x.Status == Domain.Enum.Status.Modified));
            if (model == null ||department != null)
                return IdentityResult.Failed();

            Department newDepartment = _mapper.Map<Department>(model);
            newDepartment.Status = Domain.Enum.Status.Active;
            newDepartment.CreateDate = DateTime.Now;
            if (await _departmentWriteRepository.Insert(newDepartment))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateDepartment(UpdateDepartmentDTO model)
        {
            // Id'ye göre departmanı bulur.
            Department department = await _departmentReadRepository.GetSingleDefault(x => x.Id == model.Id); 
            if (department == null)
                return IdentityResult.Failed();

            // Id'si güncellenmek istenen departmanın Id'sine eşit olmayan departmanları getirir.
            var otherDepartments = await _departmentReadRepository.GetDefaults(x=> x.Id != model.Id); 

            // Güncellenmek istenen departmanın ismi başka bir departmanda var mı diye kontrol eder.
            bool departmentExistsWithSameName = otherDepartments.Any(x => x.DepartmentName == model.DepartmentName && (x.Status == Domain.Enum.Status.Active || x.Status == Domain.Enum.Status.Modified));

            if (departmentExistsWithSameName == true)
                return IdentityResult.Failed();

            department.DepartmentName = model.DepartmentName;
            department.Description = model.Description;
            department.Status = Domain.Enum.Status.Modified;
            department.UpdateDate = DateTime.Now;

            if (await _departmentWriteRepository.Update(department))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }
    }
}
