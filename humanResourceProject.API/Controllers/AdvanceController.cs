using humanResourceProject.Application.Services.Abstract.IAdvanceServices;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvanceController : ControllerBase
    {
        private readonly IAdvanceReadService _advanceReadService;
        private readonly IAdvanceWriteService _advanceWriteService;

        public AdvanceController(IAdvanceReadService advanceReadService, IAdvanceWriteService advanceWriteService)
        {
            _advanceReadService = advanceReadService;
            _advanceWriteService = advanceWriteService;
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

        [HttpPost]
        public async Task<IActionResult> CreateAdvance([FromBody] AdvanceDTO model)
        {
            return Ok(await _advanceWriteService.InsertAdvance(model));
        }

       // [HttpGet]
       // [Route("GetUpdateAdvanceDTO/{id}")]
       // public async Task<IActionResult> GetUpdateAdvanceDTO(Guid id)
       // {
       //     return Ok(await _advanceReadService.GetUpdateAdvanceDTO(id));
       // }

        [HttpPut]
        public async Task<IActionResult> UpdateAdvance([FromBody] AdvanceDTO model)
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
