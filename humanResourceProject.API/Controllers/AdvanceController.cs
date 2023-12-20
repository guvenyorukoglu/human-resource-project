using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IAdvanceServices;
using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Domain.Entities.Concrete;
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
        private readonly IMailService _mailService;
        private readonly IAppUserReadService _appUserReadService;
        private readonly Mapper _mapper;


        public AdvanceController(IAdvanceReadService advanceReadService, IAdvanceWriteService advanceWriteService, Mapper mapper, IMailService mailService, IAppUserReadService appUserReadService = null)
        {
            _advanceReadService = advanceReadService;
            _advanceWriteService = advanceWriteService;
            _mapper = mapper;
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
        public async Task<IActionResult> UpdateStatus([FromBody] AdvanceDTO model)
        {
            AppUser user= await _appUserReadService.GetSingleDefault(x=>x.Id == model.EmployeeId);
            string action = "DAD";
            if (model.AdvanceStatus == Domain.Enum.RequestStatus.Approved)
            {
                model.AdvanceStatus = Domain.Enum.RequestStatus.Approved;
                _mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} Avansın onaylandı. Güzel günlerde kullan");

            }
            else if (model.AdvanceStatus == Domain.Enum.RequestStatus.Rejected)
            {
                model.AdvanceStatus = Domain.Enum.RequestStatus.Rejected;
                _mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} Avansın reddedildi");
            }
          

            return Ok(await _advanceWriteService.UpdateAdvance(model));
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
