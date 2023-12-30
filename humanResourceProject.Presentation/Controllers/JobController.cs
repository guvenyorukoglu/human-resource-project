using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    [Authorize(Roles = "SiteManager, CompanyManager")]
    public class JobController : Controller
    {
        private readonly HttpClient _httpClient;
        public JobController()
        {
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://monitoreaseapi.azurewebsites.net"); // Azure
            _httpClient.BaseAddress = new Uri("https://localhost:7255/");
        }
        public async Task<IActionResult> Jobs()
        {
            Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);

            var response = await _httpClient.GetAsync($"api/Job/GetJobsByCompanyId/{companyId}");

            if (response.IsSuccessStatusCode)
            {
                var jobsResponse = await response.Content.ReadAsStringAsync();
                List<JobVM>? jobsList = JsonConvert.DeserializeObject<List<JobVM>>(jobsResponse);
                return View(jobsList);
            }

            return View("Error");
        }

        public async Task<IActionResult> CreateJob()
        {
            Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);

            return View(new JobDTO { CompanyId = companyId });

        }

        [HttpPost]
        public async Task<IActionResult> CreateJob(JobDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Job/CreateJob", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Jobs));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bu pozisyon adına sahip zaten bir pozisyon var!");
                return View(model);
            }

        }

        public async Task<IActionResult> UpdateJob(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Job/GetUpdateJobDTO/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jobDTOResponse = await response.Content.ReadAsStringAsync();
                UpdateJobDTO? jobDTO = JsonConvert.DeserializeObject<UpdateJobDTO>(jobDTOResponse);
                return View(jobDTO);
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateJob(UpdateJobDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Job/", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Jobs));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bu pozisyon adına sahip zaten bir pozisyon var!");
                return View(model);
            }
        }

        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Job/DeleteJob/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Jobs));
            }

            return View("Error");
        }
    }
}
