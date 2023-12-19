
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

        [HttpPost]
        public async Task<IActionResult> InsertJob([FromBody] JobDTO model)
        {
            return Ok(await _jobWriteService.InsertJob(model));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateJob([FromBody] JobDTO model)
        {
            var result = await _jobWriteService.UpdateJob(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

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
