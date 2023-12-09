using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.API.Controllers
{
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
        public async Task<IActionResult> Employees()
        {
            return Ok(await _appUserReadService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> EmployeeById(Guid id)
        {
            return Ok(await _appUserReadService.GetSingleDefault(x => x.Id == id));
        }

        [HttpPost]
        [Route("GetEmployeesByCompanyId/{companyId}")]
        public async Task<IActionResult> GetEmployeesByCompanyId(Guid companyId)
        {
            return Ok(await _appUserReadService.GetEmployeesByCompanyId(companyId));
        }


        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdatePersonelDTO updatedPersonel)
        {
            await _appUserWriteService.UpdateEmployee(updatedPersonel);
            return Ok("Güncellenmiştir.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            await _appUserWriteService.Delete(id);
            return Ok("Silme işlemi gerçekleşti.");
        }



    }
}
