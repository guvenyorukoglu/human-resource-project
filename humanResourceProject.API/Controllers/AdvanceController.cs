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
                string body = $"Sayın {user.FirstName} {user.LastName}, {advance.CreateDate.ToShortDateString()} tarihli {advance.AmountOfAdvance} {advance.Currency.GetDisplayName()} avans talebiniz onaylannıştır. Güzel günlerde kullanınız.";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, action, subject, body);
                //_mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} Avansın onaylandı. Güzel günlerde kullan");
            }
            else if (model.AdvanceStatus == Domain.Enum.RequestStatus.Rejected)
            {
                model.AdvanceStatus = Domain.Enum.RequestStatus.Rejected;
                string subject = "Avans Reddi!";
                string body = $"Sayın {user.FirstName} {user.LastName} Avans talebiniz reddedildi.";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, action, subject, body);
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
        [Route("GetAdvancesByDepartmentId/{id}")]
        public async Task<IActionResult> GetAdvancesByDepartmentId(Guid id)
        {
            return Ok(await _advanceReadService.GetAdvancesByDepartmentId(id));
        }

        [HttpGet]
        [Route("GetAdvancesByCompanyId/{id}")]
        public async Task<IActionResult> GetAdvancesByCompanyId(Guid id)
        {
            return Ok(await _advanceReadService.GetAdvancesByCompanyId(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvance([FromBody] AdvanceDTO model)
        {
            AppUser manager = await _appUserReadService.GetSingleDefault(x => x.Id == model.Employee.ManagerId);
            string recipientEmail = manager.Email;
            string mailToName = $"{manager.FirstName} {manager.LastName}";
            string action = "";
            string subject = "Avans Talebi!";
            string body = $"Sayın {manager.FirstName} {manager.LastName}, {model.CreateDate.ToShortDateString()} tarihli {model.AmountOfAdvance} {model.Currency.GetDisplayName()} avans talebi yapılmıştır. Uygulamaya giriş yapıp onaylamanızı rica ederiz.";
            await _mailService.SendEmailAsync(manager, recipientEmail, mailToName, action, subject, body);
            return Ok(await _advanceWriteService.InsertAdvance(model));
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
