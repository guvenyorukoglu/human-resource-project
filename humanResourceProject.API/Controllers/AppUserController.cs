using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserReadService _appUserReadService;

        public AppUserController(IAppUserReadService appUserReadService)
        {
            _appUserReadService = appUserReadService;
        }

        [HttpPost]
        [Route("GetEmployeesByCompany/{companyId}")]
        public async Task<IActionResult> GetEmployeesByCompany(Guid companyId)
        {
            return Ok(await _appUserReadService.GetEmployeesByCompanyId(companyId));
        }




    }
}
