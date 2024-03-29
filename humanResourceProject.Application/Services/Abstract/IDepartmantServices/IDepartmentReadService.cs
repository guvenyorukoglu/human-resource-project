﻿using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Abstract.IDepartmantServices
{
    public interface IDepartmentReadService : IBaseReadService<Department>
    {
        Task<DepartmentDTO> GetDepartmentById(Guid id);
        Task<List<DepartmentVM>> GetAllDepartments();
        Task<List<DepartmentVM>> GetDepartmentsByCompanyId(Guid id);
        Task<UpdateDepartmentDTO> GetUpdateDepartmentDTO(Guid id);
        Task<Guid> GetIdByDepartmentName(string departmentName);
    }
}
