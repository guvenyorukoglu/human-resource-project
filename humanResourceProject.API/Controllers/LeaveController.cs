using humanResourceProject.Application.Services.Abstract.ILeaveServices;
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

        public LeaveController(ILeaveReadService leaveReadService, ILeaveWriteService leaveWriteService)
        {
            _leaveReadService = leaveReadService;
            _leaveWriteService = leaveWriteService;
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

        [HttpDelete]
        [Route("DeleteLeave/{id}")]
        public async Task<IActionResult> DeleteLeave(Guid id)
        {
            return Ok(await _leaveWriteService.DeleteLeave(id));
        }
    }
}
