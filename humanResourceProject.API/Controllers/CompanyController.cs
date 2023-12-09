using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyReadService _companyReadService;
        private readonly ICompanyWriteService _companyWriteService;

        public CompanyController(ICompanyReadService companyReadService, ICompanyWriteService companyWriteService)
        {
            _companyReadService = companyReadService;
            _companyWriteService = companyWriteService;
        }

        [HttpGet]
        public async Task<IActionResult> Companies()
        {
            return Ok(await _companyReadService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> CompanyById(Guid id)
        {
            return Ok(await _companyReadService.GetSingleDefault(x => x.Id == id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDTO newCompany)
        {
            await _companyWriteService.CreateCompany(newCompany);
            return Ok("Şirket oluşturuldu.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyDTO updatedCompany)
        {
            await _companyWriteService.UpdateCompany(updatedCompany);
            return Ok("Güncellenmiştir.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _companyWriteService.Delete(id);
            return Ok("Silme işlemi gerçekleşti.");
        }
    }
}
