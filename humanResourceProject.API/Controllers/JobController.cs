using humanResourceProject.Application.Services.Abstract.IJobServices;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : Controller
    {
      
        private readonly IJobReadService _jobReadService;
        private readonly IJobWriteService _jobWriteService;

        public JobController( IJobReadService jobReadService, IJobWriteService jobWriteService)
        {
            _jobReadService = jobReadService;
            _jobWriteService = jobWriteService;
        }

        [HttpGet]
        [Route("GetJobById/{id}")]
        public async Task<IActionResult> GetJobById(Guid id)
        {
            return Ok(await _jobReadService.GetJobById(id));
        }

        [HttpGet]
        [Route("GetAllJobs")]
        public async Task<IActionResult> GetAllJobs()
        {
            return Ok(await _jobReadService.GetAllJobs());
        }

        [HttpGet]
        [Route("GetJobsByCompanyId/{companyId}")]
        public async Task<IActionResult> GetJobsByCompanyId(Guid companyId)
        {
            return Ok(await _jobReadService.GetJobsByCompanyId(companyId));
        }

        [HttpPost]
        [Route("CreateJob")]
        public async Task<IActionResult> CreateJob([FromBody] JobDTO model)
        {
            var result = await _jobWriteService.InsertJob(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Yeni pozisyon oluşturuldu.");
        }

        [HttpGet]
        [Route("GetUpdateJobDTO/{id}")]
        public async Task<IActionResult> GetUpdateJobDTO(Guid id) // Departman güncelleme için DTO döndürür.
        {
            return Ok(await _jobReadService.GetUpdateJobDTO(id));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateJob([FromBody] UpdateJobDTO model)
        {
            var result = await _jobWriteService.UpdateJob(model);
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
        [Route("DeleteJob/{id}")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var result = await _jobWriteService.DeleteJob(id);
            if (!result.Succeeded)
                return BadRequest();

            return Ok("Silme işlemi gerçekleşti.");
        }
    }
}
