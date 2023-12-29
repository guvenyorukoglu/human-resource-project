using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IJobServices;
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
        [Route("GetManagersByDepartmentId/{id}")]
        public async Task<IActionResult> GetManagersByDepartmentId(Guid id) // Departman Id'sine göre Yönetici
        {
            var employees = await _appUserReadService.GetManagersByDepartmentId(id);
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
        [Route("GetManagersByCompanyId/{id}")]
        public async Task<IActionResult> GetManagersByCompanyId(Guid id) // Şirket Id'sine göre Yöneticiler
        {
            return Ok(await _appUserReadService.GetManagersByCompanyId(id));
        }

        [HttpGet]
        [Route("GetUpdateUserDTO/{id}")]
        public async Task<IActionResult> GetUpdateUserDTO(Guid id) // Personel Güncellemek için View'e DTO gönderir
        {
            return Ok(await _appUserWriteService.GetUpdateUserDTOById(id));
        }
        [HttpGet]
        [Route("GetUpdateProfileDTO/{id}")]
        public async Task<IActionResult> GetUpdateProfileDTO(Guid id) // Personel Güncellemek için View'e DTO gönderir
        {
            return Ok(await _appUserWriteService.GetUpdateProfileDTOById(id));
        }

        [HttpPost]
        [Route("GetEmployeesByCompanyId")]
        public async Task<IActionResult> GetEmployeesByCompanyId([FromBody] Guid companyId) // Şirket Id'sine göre Personeller
        {
            return Ok(await _appUserReadService.GetEmployeesByCompanyId(companyId));
        }

        [HttpPost]
        [Route("GetEmployeesByManagerId")]
        public async Task<IActionResult> GetEmployeesByManagerId([FromBody] Guid managerId)  // Yönetici Id'sine göre Personeller
        {
            return Ok(await _appUserReadService.GetEmployeesByManagerId(managerId));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonel([FromBody] CreateEmployeeDTO model) // Yeni Personel Oluşturma
        {
            IdentityResult result = await _appUserWriteService.RegisterPersonel(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);


            return RedirectToAction("ForgotPassword", "Account", new { email = model.Email });
        }

        [HttpPost]
        [Route("CreatePersonelManager")]
        public async Task<IActionResult> CreatePersonelManager([FromBody] CreateEmployeeDTO model)
        {
            IdentityResult result = await _appUserWriteService.RegisterPersonelManager(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);


            return RedirectToAction("ForgotPassword", "Account", new { email = model.Email });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateUserDTO updatedPersonel) // Personel Güncelleme
        {
            var result = await _appUserWriteService.Update(updatedPersonel);

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


        [HttpPut]
        [Route("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO updatedProfile) // Personel Profili Güncelleme
        {
            var result = await _appUserWriteService.UpdateProfile(updatedProfile);
           
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Güncellenmiştir.");
        }


        [HttpGet]
        [Route("FireEmployee/{id}")]
        public async Task<IActionResult> FireEmployee(Guid id) // Personel İşten Çıkarma
        {
            var result = await _appUserWriteService.FireEmployee(id);

            if (!result.Succeeded)
                return BadRequest();

            return Ok("İşten çıkarma işlemi gerçekleşti.");
        }

        [HttpGet]
        [Route("ProfileEmployee/{id}")]
        public async Task<IActionResult> ProfileEmployee(Guid id) //  Personel Bilgileri
        {
            return Ok(await _appUserReadService.ProfileEmployee(id));
        }

        [HttpGet]

        [Route("GetAppUserVM/{id}")]
        public async Task<IActionResult> GetAppUserVM(Guid id)
        {
            return Ok(await _appUserReadService.GetAppUserVM(id));

        [Route("ProfileCompanyManager/{id}")]
        public async Task<IActionResult> ProfileCompanyManager(Guid id) //  Personel Bilgileri
        {
            return Ok(await _appUserReadService.ProfileCompanyManager(id));
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
