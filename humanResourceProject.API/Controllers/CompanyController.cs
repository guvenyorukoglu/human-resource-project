using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Domain.Entities.Concrete;
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
        private readonly IMailService _mailService;
        private readonly IAppUserReadService _appUserReadService;
        private readonly IAppUserWriteService _appUserWriteService;

        public CompanyController(ICompanyReadService companyReadService, ICompanyWriteService companyWriteService, IAppUserReadService appUserReadService, IMailService mailService, IAppUserWriteService appUserWriteService)
        {
            _companyReadService = companyReadService;
            _companyWriteService = companyWriteService;
            _appUserReadService = appUserReadService;
            _mailService = mailService;
            _appUserWriteService = appUserWriteService;
        }

        [HttpGet]
        public async Task<IActionResult> Companies()
        {
            var list = await _companyReadService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> CompanyById(Guid id)
        {
            return Ok(await _companyReadService.GetSingleDefault(x => x.Id == id));
        }

        [HttpGet]
        [Route("GetCompanyVM/{id}")]
        public async Task<IActionResult> GetCompanyVM(Guid id)
        {
            return Ok(await _companyReadService.GetCompanyVM(id));
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

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var result = await _companyWriteService.Delete(id);
            if (!result)
                return BadRequest();

            return Ok("Silme işlemi gerçekleşti.");
        }

        [HttpPut]
        [Route("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateCompanyDTO model)
        {
            //var model = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateCompanyDTO>(model);

            var result = await _companyWriteService.Update(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            Company company = await _companyReadService.GetSingleDefault(x => x.Id == model.Id);
            AppUser user = await _appUserReadService.GetSingleDefault(x => x.CompanyId == company.Id);
            string recipientEmail = user.Email;
            string mailToName = $"{user.FirstName} {user.LastName}";
            if (company.CompanyStatus == Domain.Enum.RequestStatus.Approved)
            {
                user.Status = Domain.Enum.Status.Active;
                await _appUserWriteService.Update(user);
                string subject = "Şirket Onayı!";
                string body = $"<p>Sayın {user.FirstName} {user.LastName},</p><p>{company.CreateDate.ToShortDateString()} tarihli  {company.CompanyName} adlı şirket talebiniz onaylanmıştır.</p><p>Güzel günlerde kullanmanız dileğiyle.</p><br><hr><br><h3>Team Monitorease</h3>";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);

                 
            }
            else if (model.CompanyStatus == Domain.Enum.RequestStatus.Rejected)
            {
                user.Status = Domain.Enum.Status.Inactive;
                await _appUserWriteService.Update(user);
                model.CompanyStatus = Domain.Enum.RequestStatus.Rejected;
                string subject = "Şirket Reddi!";
                string body = $"<p>Sayın {user.FirstName} {user.LastName},</p><p>{company.CreateDate.ToShortDateString()} tarihli {company.CompanyName} adlı şirket talebiniz reddedilmiştir.</p><p>Yöneticinin reddetme sebebi:</p><p>{model.RejectReason}</p><br><hr><br><h3>Team Monitorease</h3>";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);
                
            }

            return Ok();
        }
    }
}
