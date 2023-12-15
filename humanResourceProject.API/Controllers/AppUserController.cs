using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace humanResourceProject.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserReadService _appUserReadService;
        private readonly IAppUserWriteService _appUserWriteService;
        private readonly IMailService _mailService;
        private readonly UserManager<AppUser>  _userManager;

        public AppUserController(IAppUserReadService appUserReadService, IAppUserWriteService appUserWriteService, UserManager<AppUser> userManager, IMailService mailService)
        {
            _appUserReadService = appUserReadService;
            _appUserWriteService = appUserWriteService;
            _userManager = userManager;
            _mailService = mailService;
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
         public async Task<IActionResult> CreatePersonel([FromBody] UserRegisterDTO model) // Yeni Personel Oluşturma
        {
            IdentityResult result = await _appUserWriteService.RegisterPersonel(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            AppUser user = await _userManager.FindByEmailAsync(model.Email);
            string token =await _userManager.GenerateEmailConfirmationTokenAsync(user);

            string url = "https://localhost:7255/Account/ResetPassword";
            string action =url+"?"+"id="+user.Id+"&"+"token="+token;
            await _mailService.SendMessageAsync(user, action);

            return Ok("Yeni personel oluşturuldu.");
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
      


        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {

            AppUser user = await _userManager.FindByIdAsync(model.Id.ToString());
            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

            return Ok();
        }




    }
}
