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
        [Route("GetLeavesByEmployeeId/{id}")]
        public async Task<IActionResult> GetLeavesByEmployeeId(Guid id)
        {
            return Ok(await _leaveReadService.GetLeavesByEmployeeId(id));
        }

        [HttpGet]
        [Route("GetLeavesByDepartmentId/{id}")]
        public async Task<IActionResult> GetLeavesByDepartmentId(Guid id)
        {
            return Ok(await _leaveReadService.GetLeavesByDepartmentId(id));
        }



        [HttpPost]
        public async Task<IActionResult> CreateLeave([FromBody] LeaveDTO model)
        {
            return Ok(await _leaveWriteService.InsertLeave(model));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLeave([FromBody] LeaveDTO model)
        {
            return Ok(await _leaveWriteService.UpdateLeave(model));
        }


        [HttpPut]
        [Route("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] LeaveDTO model)
        {
            AppUser user = await _appUserReadService.GetSingleDefault(x => x.Id == model.EmployeeId);
            string action = "DENEMELİK";
            if (model.LeaveStatus == Domain.Enum.RequestStatus.Approved)
            {
                model.LeaveStatus = Domain.Enum.RequestStatus.Approved;
                _mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} İznin onaylandı. Güzel günler dileriz.");

            }
            else if (model.LeaveStatus == Domain.Enum.RequestStatus.Rejected)
            {
                model.LeaveStatus = Domain.Enum.RequestStatus.Rejected;
                _mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} İznin maalesef reddedildi");
            }


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
