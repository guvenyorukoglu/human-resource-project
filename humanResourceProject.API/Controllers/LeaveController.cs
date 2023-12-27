using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.ILeaveServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveReadService _leaveReadService;
        private readonly ILeaveWriteService _leaveWriteService;
        private readonly IAppUserReadService _appUserReadService;
        private readonly IMailService _mailService;

        public LeaveController(ILeaveReadService leaveReadService, ILeaveWriteService leaveWriteService, IAppUserReadService appUserReadService, IMailService mailService)
        {
            _leaveReadService = leaveReadService;
            _leaveWriteService = leaveWriteService;
            _appUserReadService = appUserReadService;
            _mailService = mailService;
        }

        [HttpGet]
        [Route("GetAllLeaves")]
        public async Task<IActionResult> GetAllLeave()
        {
            return Ok(await _leaveReadService.GetAllLeaves());
        }

        [HttpGet]
        [Route("GetLeaveDTO/{employeeId}")]
        public async Task<IActionResult> GetLeaveDTO(Guid employeeId)
        {
            return Ok(await _leaveReadService.GetLeaveDTO(employeeId));
        }

        [HttpPut]
        [Route("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateLeaveDTO model)
        {
            var result = await _leaveWriteService.UpdateLeave(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            Leave leave = await _leaveReadService.GetSingleDefault(x => x.Id == model.Id);
            AppUser user = await _appUserReadService.GetSingleDefault(x => x.Id == model.EmployeeId);
            string action = "";
            string recipientEmail = user.Email;
            string mailToName = $"{user.FirstName} {user.LastName}";
            if (model.LeaveStatus == Domain.Enum.RequestStatus.Approved)
            {
                model.LeaveStatus = Domain.Enum.RequestStatus.Approved;
                string subject = "İzin Onayı!";
                string body = $"<p>Sayın {user.FirstName} {user.LastName},</p><p>İzin talebiniz onaylandı.</p><p>Güzel günler dileriz.</p><br><hr><br><h3>Team Monitorease</h3>";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);
                //_mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} İznin onaylandı. Güzel günler dileriz.");

            }
            else if (model.LeaveStatus == Domain.Enum.RequestStatus.Rejected)
            {
                model.LeaveStatus = Domain.Enum.RequestStatus.Rejected;
                string subject = "İzin Reddi!";
                string body = $"<p>Sayın {user.FirstName} {user.LastName},</p><p>İzin talebiniz reddedildi.</p><p>İyi çalışmalar dileriz.</p><br><hr><br><h3>Team Monitorease</h3>";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);
                //_mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} İznin maalesef reddedildi");
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetLeavesByEmployeeId/{id}")]
        public async Task<IActionResult> GetLeavesByEmployeeId(Guid id)
        {
            return Ok(await _leaveReadService.GetLeavesByEmployeeId(id));
        }

        [HttpGet]
        [Route("GetLeavesByManagerId/{id}")]
        public async Task<IActionResult> GetLeavesByManagerId(Guid id)
        {
            return Ok(await _leaveReadService.GetLeavesByManagerId(id));
        }

        [HttpGet]
        [Route("GetLeavesByCompanyId/{id}")]
        public async Task<IActionResult> GetLeavesByCompanyId(Guid id)
        {
            return Ok(await _leaveReadService.GetLeavesByCompanyId(id));
        }

        [HttpGet]
        [Route("GetLeaveDTO/{employeeId}")]
        public async Task<IActionResult> GetLeaveDTO(Guid employeeId)
        {
            return Ok(await _leaveReadService.GetLeaveDTO(employeeId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeave([FromBody] LeaveDTO model)
        {
            var result = await _leaveWriteService.InsertLeave(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            AppUser employee = await _appUserReadService.GetSingleDefault(x => x.Id == model.EmployeeId);
            AppUser manager = await _appUserReadService.GetSingleDefault(x => x.Id == employee.ManagerId);
            string recipientEmail = manager.Email;
            string mailToName = $"{manager.FirstName} {manager.LastName}";
            string action = "";
            string subject = "İzin Talebi!";
            string body = $"<p>Sayın {manager.FirstName} {manager.LastName},</p><p>{employee.FirstName} {employee.LastName} tarafından {model.CreateDate.ToShortDateString()} tarihli {model.DaysOfLeave} gün izin talebi yapılmıştır.</p><p>Uygulama üzerinden onaylama ya da reddetme işlemini yapabilirsiniz.</p><p>İyi çalışmalar dileriz.</p><br><hr><br><h3>Team Monitorease</h3>";
            await _mailService.SendEmailAsync(manager, recipientEmail, mailToName, subject, body);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetUpdateLeaveDTO/{id}")]
        public async Task<IActionResult> GetUpdateLeaveDTO(Guid id)
        {
            return Ok(await _leaveReadService.GetUpdateLeaveDTO(id));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLeave([FromBody] UpdateLeaveDTO model)
        {
            return Ok(await _leaveWriteService.UpdateLeave(model));
        }

        [HttpDelete]
        [Route("DeleteLeave/{id}")]
        public async Task<IActionResult> DeleteLeave(Guid id)
        {
            return Ok(await _leaveWriteService.DeleteLeave(id));
        }
    }
}
