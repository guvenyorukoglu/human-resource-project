using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IJobServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserReadService _appUserReadService;
        private readonly IAppUserWriteService _appUserWriteService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJobReadService _jobReadServices;

        public AppUserController(IAppUserReadService appUserReadService, IAppUserWriteService appUserWriteService, UserManager<AppUser> userManager, IJobReadService jobReadServices)
        {
            _appUserReadService = appUserReadService;
            _appUserWriteService = appUserWriteService;
            _userManager = userManager;
            _jobReadServices = jobReadServices;
        }

        [HttpGet]
        public async Task<IActionResult> Employees() // Tüm Personeller
        {
            return Ok(await _appUserReadService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> EmployeeById(Guid id) // Id'ye göre Personel
        {
            return Ok(await _appUserReadService.GetSingleDefault(x => x.Id == id));
        }
        [HttpGet]
        [Route("GetManagerByDepartmentId/{id}")]
        public async Task<IActionResult> GetManagerByDepartmentId(Guid id) // Departman Id'sine göre Yönetici
        {
            var employees = await _appUserReadService.GetEmployeesByDepartmentId(id);
            foreach (var employee in employees)
            {
                AppUser appUser = await _userManager.FindByIdAsync(employee.Id.ToString());
                if (await _userManager.IsInRoleAsync(appUser, "Manager"))
                {
                    return Ok(employee);
                }
            }            
            return Ok();
        }

        [HttpGet]
        [Route("GetUpdateUserDTO/{id}")]
        public async Task<IActionResult> GetUpdateUserDTO(Guid id) // Personel Güncellemek için View'e DTO gönderir
        {
            return Ok(await _appUserWriteService.GetUpdateUserDTOById(id));
        }

        [HttpPost]
        [Route("GetEmployeesByCompanyId")]
        public async Task<IActionResult> GetEmployeesByCompanyId([FromBody] Guid companyId) // Şirket Id'sine göre Personeller
        {
            return Ok(await _appUserReadService.GetEmployeesByCompanyId(companyId));
        }

        [HttpPost]
        [Route("GetEmployeesByDepartmentId")]
        public async Task<IActionResult> GetEmployeesByDepartmentId([FromBody] Guid departmentId) // Şirket Id'sine göre Personeller
        {
            return Ok(await _appUserReadService.GetEmployeesByDepartmentId(departmentId));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonel([FromBody] CreateEmployeeDTO model) // Yeni Personel Oluşturma
        {
            IdentityResult result = await _appUserWriteService.RegisterPersonel(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);


            return RedirectToAction("ForgotPassword", "Account", new {email = model.Email});
        }

        [HttpPost]
        [Route("CreatePersonelManager")]
        public async Task<IActionResult> CreatePersonelManager([FromBody] CreateEmployeeDTO model) 
        {
            IdentityResult result = await _appUserWriteService.RegisterPersonelManager(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);


            return RedirectToAction("ForgotPassword", "Account", new {email=model.Email});
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateUserDTO updatedPersonel) // Personel Güncelleme
        {
            var result = await _appUserWriteService.Update(updatedPersonel);
            var jobs = await _jobReadServices.GetAllJobs();
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Güncellenmiştir.");
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id) // Personel Silme
        {
            var result = await _appUserWriteService.Delete(id);
            if (!result)
                return BadRequest();

            return Ok("Silme işlemi gerçekleşti.");
        }




        //[HttpPost]
        //[Route("ResetPassword")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        //{

        //    AppUser user = await _userManager.FindByIdAsync(model.Id.ToString());
        //    var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
        //    var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

        //    return Ok();
        //}
    }
}
