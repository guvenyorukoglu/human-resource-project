using humanResourceProject.Application.Services.Abstract.IPossessionLogServices;
using humanResourceProject.Application.Services.Abstract.IPossessionServices;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PossessionController : ControllerBase
    {
        private readonly IPossessionReadService _possessionReadService;
        private readonly IPossessionWriteService _possessionWriteService;
        private readonly IPossessionLogReadService _possessionLogReadService;
        private readonly IPossessionLogWriteService _possessionLogWriteService;

        public PossessionController(IPossessionWriteService possessionWriteService, IPossessionReadService possessionReadService, IPossessionLogReadService possessionLogReadService, IPossessionLogWriteService possessionLogWriteService)
        {
            _possessionWriteService = possessionWriteService;
            _possessionReadService = possessionReadService;
            _possessionLogReadService = possessionLogReadService;
            _possessionLogWriteService = possessionLogWriteService;
        }

        [HttpGet]
        [Route("GetPossessionById/{id}")]
        public async Task<IActionResult> GetPossessionById(Guid id)
        {
            return Ok(await _possessionReadService.GetPossessionById(id));
        }

        [HttpGet]
        [Route("GetAllPossessions")]
        public async Task<IActionResult> GetAllPossessions()
        {
            return Ok(await _possessionReadService.GetAllPossessions());
        }

        [HttpGet]
        [Route("GetPossessionsByCompanyId/{companyId}")]
        public async Task<IActionResult> GetPossessionsByCompanyId(Guid companyId)
        {
            return Ok(await _possessionReadService.GetPossessionsByCompanyId(companyId));
        }

        [HttpGet]
        [Route("GetPossessionsByEmployeeId/{employeeId}")]
        public async Task<IActionResult> GetPossessionsByEmployeeId(Guid employeeId)
        {
            return Ok(await _possessionLogReadService.GetPossessionsByEmployeeId(employeeId));
        }

        [HttpPost]
        [Route("CreatePossession")]
        public async Task<IActionResult> CreatePossession([FromBody] PossessionDTO model)
        {
            var result = await _possessionWriteService.InsertPossession(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Yeni zimmet oluşturuldu.");
        }

        [HttpGet]
        [Route("GetUpdatePossessionDTO/{id}")]
        public async Task<IActionResult> GetUpdatePossessionDTO(Guid id) // Zimmet güncelleme için DTO döndürür.
        {
            return Ok(await _possessionReadService.GetUpdatePossessionDTO(id));
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePossession([FromBody] UpdatePossessionDTO model)
        {
            var result = await _possessionWriteService.UpdatePossession(model);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok("Güncellenmiştir.");
        }

        [HttpDelete]
        [Route("DeletePossession/{id}")]
        public async Task<IActionResult> DeletePossession(Guid id)
        {
            return Ok(await _possessionWriteService.DeletePossession(id));
        }

        [HttpPost]
        [Route("AssignPossession")]
        public async Task<IActionResult> AssignPossession([FromBody] AssignPossessionDTO model)
        {
            if (model.EmployeeId == Guid.Empty || model.PossessionId == Guid.Empty)
            {
                return BadRequest("Zimmet ataması yapılırken bir hata oluştu.");
            }

            var result = await _possessionLogWriteService.AssignPossession(model);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result.Succeeded);
        }

        [HttpPost]
        [Route("TakeBackPossession")]
        public async Task<IActionResult> TakeBackPossession([FromBody] Guid id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest("Zimmet geri alma işlemi yapılırken bir hata oluştu.");
            }

            var result = await _possessionLogWriteService.TakeBackPossession(id);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result.Succeeded);
        }
    }
}
