using humanResourceProject.Application.Services.Abstract.IAppUserServices;
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

        public AppUserController(IAppUserReadService appUserReadService, IAppUserWriteService appUserWriteService)
        {
            _appUserReadService = appUserReadService;
            _appUserWriteService = appUserWriteService;
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id) // Personel Silme
        {
            var result = await _appUserWriteService.Delete(id);
            if (!result)
                return BadRequest();

            return Ok("Silme işlemi gerçekleşti.");
        }



    }
}
