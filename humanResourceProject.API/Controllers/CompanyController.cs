using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        [HttpGet]
        [Route("GetUpdateCompanyDTO/{id}")]
        public async Task<IActionResult> GetUpdateCompanyDTO(Guid id) // Şirket Güncellemek için View'e DTO gönderir
        {
            return Ok(await _companyWriteService.GetUpdateCompanyDTOById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyRegisterDTO model)
        {
            IdentityResult result = await _companyWriteService.RegisterCompany(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var company = await _companyReadService.GetSingleDefault(x => x.CompanyName == model.CompanyName);

            return Ok(company.Id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyDTO updatedCompany)
        {
            var result = await _companyWriteService.Update(updatedCompany);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Güncellenmiştir.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var result = await _companyWriteService.Delete(id);
            if (!result)
                return BadRequest();

            return Ok("Silme işlemi gerçekleşti.");
        }
    }
}
