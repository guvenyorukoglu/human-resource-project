using humanResourceProject.Application.Services.Abstract.IAdvanceServices;
using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace humanResourceProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvanceController : ControllerBase
    {
        private readonly IAdvanceReadService _advanceReadService;
        private readonly IAdvanceWriteService _advanceWriteService;
        private readonly IMailService _mailService;
        private readonly IAppUserReadService _appUserReadService;

        public AdvanceController(IAdvanceReadService advanceReadService, IAdvanceWriteService advanceWriteService, IMailService mailService, IAppUserReadService appUserReadService)
        {
            _advanceReadService = advanceReadService;
            _advanceWriteService = advanceWriteService;
            _mailService = mailService;
            _appUserReadService = appUserReadService;
        }

        [HttpGet]
        [Route("GetAllAdvances")]
        public async Task<IActionResult> GetAllAdvances()
        {
            return Ok(await _advanceReadService.GetAllAdvances());
        }

        [HttpPut]
        [Route("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateAdvanceDTO model)
        {
            var result = await _advanceWriteService.UpdateAdvance(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            Advance advance = await _advanceReadService.GetSingleDefault(x => x.Id == model.Id);
            AppUser user = await _appUserReadService.GetSingleDefault(x => x.Id == advance.EmployeeId);
            string action = "";
            string recipientEmail = user.Email;
            string mailToName = $"{user.FirstName} {user.LastName}";
            if (advance.AdvanceStatus == Domain.Enum.RequestStatus.Approved)
            {
                string subject = "Avans Onayı!";
                string body = $"<p>Sayın {user.FirstName} {user.LastName},</p><p>{advance.CreateDate.ToShortDateString()} tarihli {advance.AmountOfAdvance} {advance.Currency.GetDisplayName()} avans talebiniz onaylannıştır.</p><p>Güzel günlerde kullanmanız dileğiyle.</p><br><hr><br><h3>Team Monitorease</h3>";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);
                //_mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} Avansın onaylandı. Güzel günlerde kullan");
            }
            else if (model.AdvanceStatus == Domain.Enum.RequestStatus.Rejected)
            {
                model.AdvanceStatus = Domain.Enum.RequestStatus.Rejected;
                string subject = "Avans Reddi!";
                string body = $"<p>Sayın {user.FirstName} {user.LastName},</p><p>{advance.CreateDate.ToShortDateString()} tarihli {advance.AmountOfAdvance} {advance.Currency.GetDisplayName()} avans talebiniz reddedilmiştir.</p><br><hr><br><h3>Team Monitorease</h3>";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);
                //_mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} Avansın reddedildi");
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetAdvancesByEmployeeId/{id}")]
        public async Task<IActionResult> GetAdvancesByEmployeeId(Guid id)
        {
            return Ok(await _advanceReadService.GetAdvancesByEmployeeId(id));
        }

        [HttpGet]
        [Route("GetAdvancesByManagerId/{id}")]
        public async Task<IActionResult> GetAdvancesByManagerId(Guid id)
        {
            return Ok(await _advanceReadService.GetAdvancesByManagerId(id));
        }

        [HttpGet]
        [Route("GetAdvancesByCompanyId/{id}")]
        public async Task<IActionResult> GetAdvancesByCompanyId(Guid id)
        {
            return Ok(await _advanceReadService.GetAdvancesByCompanyId(id));
        }

        [HttpGet]
        [Route("GetAdvanceDTO/{employeeId}")]
        public async Task<IActionResult> GetAdvanceDTO(Guid employeeId)
        {
            return Ok(await _advanceReadService.GetAdvanceDTO(employeeId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvance([FromBody] AdvanceDTO model)
        {
            var result = await _advanceWriteService.InsertAdvance(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            AppUser employee = await _appUserReadService.GetSingleDefault(x => x.Id == model.EmployeeId);
            AppUser manager = await _appUserReadService.GetSingleDefault(x => x.Id == employee.ManagerId);

            string recipientEmail = manager.Email;
            string mailToName = $"{manager.FirstName} {manager.LastName}";
            string action = "";
            string subject = "Avans Talebi!";
            string body = $"<p>Sayın {manager.FirstName} {manager.LastName},</p><p>{employee.FirstName} {employee.LastName} tarafından {model.CreateDate.ToShortDateString()} tarihinde {model.AmountOfAdvance} {model.Currency.GetDisplayName()} avans talebi yapılmıştır.</p><p>Uygulamaya giriş yapıp onaylamanızı rica ederiz.</p><br><hr><br><h3>Team Monitorease</h3>";
            await _mailService.SendEmailAsync(manager, recipientEmail, mailToName, subject, body);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetUpdateAdvanceDTO/{id}")]
        public async Task<IActionResult> GetUpdateAdvanceDTO(Guid id)
        {
            return Ok(await _advanceReadService.GetUpdateAdvanceDTO(id));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAdvance([FromBody] UpdateAdvanceDTO model)
        {
            return Ok(await _advanceWriteService.UpdateAdvance(model));
        }

        [HttpDelete]
        [Route("DeleteAdvance/{id}")]
        public async Task<IActionResult> DeleteAdvance(Guid id)
        {
            return Ok(await _advanceWriteService.DeleteAdvance(id));
        }
    }
}
