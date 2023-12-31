using humanResourceProject.Application.Services.Abstract.IDepartmantServices;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
       
        private readonly IDepartmentReadService _departmentReadService;
        private readonly IDepartmentWriteService _departmentWriteService;

        public DepartmentController(IDepartmentWriteService departmentWriteService, IDepartmentReadService departmentReadService)
        {
            _departmentWriteService = departmentWriteService;
            _departmentReadService = departmentReadService;
        }

        [HttpGet]
        [Route("GetDepartmentById/{id}")]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            return Ok(await _departmentReadService.GetDepartmentById(id));
        }

        [HttpGet]
        [Route("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            return Ok(await _departmentReadService.GetAllDepartments());
        }

        [HttpGet]
        [Route("GetDepartmentsByCompanyId/{companyId}")]
        public async Task<IActionResult> GetDepartmentsByCompanyId(Guid companyId)
        {
            return Ok(await _departmentReadService.GetDepartmentsByCompanyId(companyId));
        }

        [HttpPost]
        [Route("CreateDepartment")]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDTO model)
        {
            var result = await _departmentWriteService.InsertDepartment(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Yeni departman oluşturuldu.");
        }

        [HttpGet]
        [Route("GetUpdateDepartmentDTO/{id}")]
        public async Task<IActionResult> GetUpdateDepartmentDTO(Guid id) // Departman güncelleme için DTO döndürür.
        {
            return Ok(await _departmentReadService.GetUpdateDepartmentDTO(id));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentDTO model)
        {
            var result = await _departmentWriteService.UpdateDepartment(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Güncellenmiştir.");
        }

        [HttpDelete]
        [Route("DeleteDepartment/{id}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            return Ok(await _departmentWriteService.DeleteDepartment(id));
        }
    }
}
