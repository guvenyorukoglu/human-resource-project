using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.ILeaveServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
        private readonly IConfiguration _configuration;

        public LeaveController(ILeaveReadService leaveReadService, ILeaveWriteService leaveWriteService, IAppUserReadService appUserReadService, IMailService mailService, IConfiguration configuration)
        {
            _leaveReadService = leaveReadService;
            _leaveWriteService = leaveWriteService;
            _appUserReadService = appUserReadService;
            _mailService = mailService;
            _configuration = configuration;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
        }

        [HttpGet]
        [Route("GetAllLeaves")]
        public async Task<IActionResult> GetAllLeave()
        {
            return Ok(await _leaveReadService.GetAllLeaves());
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
            AppUser manager = await _appUserReadService.GetSingleDefault(x => x.Id == user.ManagerId);
            string recipientEmail = user.Email;
            string mailToName = $"{user.FirstName} {user.LastName}";
            if (model.LeaveStatus == Domain.Enum.RequestStatus.Approved)
            {
                model.LeaveStatus = Domain.Enum.RequestStatus.Approved;
                string subject = "İzin Onayı!";
                string body = $"<p>Sayın {user.FirstName} {user.LastName},</p><p>{leave.CreateDate.ToShortDateString()} tarihinde oluşturduğunuz, {model.StartDateOfLeave.ToShortDateString()} ve {model.EndDateOfLeave.ToShortDateString()} tarihleri arasında ve {leave.LeaveNo} numaralı {model.DaysOfLeave} günlük izin talebiniz onaylanmıştır.</p><p>İyi tatiller dileriz.</p><br><hr><br><h3>Team Monitorease</h3>";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);
                //_mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} İznin onaylandı. Güzel günler dileriz.");

            }
            else if (model.LeaveStatus == Domain.Enum.RequestStatus.Rejected)
            {
                model.LeaveStatus = Domain.Enum.RequestStatus.Rejected;
                string subject = "İzin Reddi!";
                string body = $"<p>Sayın {user.FirstName} {user.LastName},</p><p>{leave.CreateDate.ToShortDateString()} tarihinde oluşturduğunuz, {model.StartDateOfLeave.ToShortDateString()} ve {model.EndDateOfLeave.ToShortDateString()} tarihleri arasında ve {leave.LeaveNo} numaralı {model.DaysOfLeave} günlük izin talebiniz; yöneticiniz {manager.FirstName} {manager.LastName} tarafından reddedildi.</p><p>Yöneticinizin reddetme sebebi:</p><p>{model.RejectReason}</p><p>İyi çalışmalar dileriz.</p><br><hr><br><h3>Team Monitorease</h3>";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);
                //_mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} İznin maalesef reddedildi");
            }

            return Ok("success");
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
            string subject = "İzin Talebi!";
            string body = $"<p>Sayın {manager.FirstName} {manager.LastName},</p><p>{employee.FirstName} {employee.LastName} tarafından {DateTime.Now.ToShortDateString()} tarihinde, {model.StartDateOfLeave.ToShortDateString()} ve {model.EndDateOfLeave.ToShortDateString()} tarihleri arasında {model.DaysOfLeave} günlük izin talebi yapılmıştır.</p><p>Uygulamaya giriş yaparak onaylama ya da reddetme işlemini yapabilirsiniz.</p><p>{_configuration["HomePage"]}</p><p>İyi çalışmalar dileriz.</p><br><hr><br><h3>Team Monitorease</h3>";
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
            var result = await _leaveWriteService.UpdateLeave(model);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            Leave leave = await _leaveReadService.GetSingleDefault(x => x.Id == model.Id);
            AppUser employee = await _appUserReadService.GetSingleDefault(x => x.Id == model.EmployeeId);
            AppUser manager = await _appUserReadService.GetSingleDefault(x => x.Id == employee.ManagerId);
            string recipientEmail = manager.Email;
            string mailToName = $"{manager.FirstName} {manager.LastName}";
            string action = "";
            string subject = "İzin Güncellendi!";
            string body = $"<p>Sayın {manager.FirstName} {manager.LastName},</p><p>{employee.FirstName} {employee.LastName} tarafından {DateTime.Now.ToShortDateString()} tarihinde, {leave.LeaveNo} numaralı izin talebinde güncelleme yapılmıştır.</p><p>Uygulamaya giriş yaparak onaylama ya da reddetme işlemini yapabilirsiniz.</p><p>{_configuration["HomePage"]}</p><p>İyi çalışmalar dileriz.</p><br><hr><br><h3>Team Monitorease</h3>";
            await _mailService.SendEmailAsync(manager, recipientEmail, mailToName, subject, body);
            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteLeave/{id}")]
        public async Task<IActionResult> DeleteLeave(Guid id)
        {
            return Ok(await _leaveWriteService.DeleteLeave(id));
        }

        [HttpGet]
        [Route("FillDashboardLeaveVM/{id}")]
        public async Task<IActionResult> FillDashboardLeaveVM(Guid id)
        {
            return Ok(await _leaveReadService.FillDashboardLeaveVM(id));
        }

        [HttpGet]
        [Route("GetLeaveDTO/{employeeId}")]
        public async Task<IActionResult> GetLeaveDTO(Guid employeeId)

        {
            return Ok(await _leaveReadService.GetLeaveDTO(employeeId));
        }
    }
}
