using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    public class AdvanceController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdvanceController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Advances()
        {
            Guid employeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var json = JsonConvert.SerializeObject(employeeId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Advance/GetAdvancesByEmployeeId/", content);
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var advances = JsonConvert.DeserializeObject<List<AdvancePersonnelVM>>(cont);
                return View(advances);

            }
            return View();
        }


        public async Task<IActionResult> AdvancesDepartment() 
        {
            Guid depatmentId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "DepartmentId").Value);
            var json = JsonConvert.SerializeObject(depatmentId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Advance/GetAdvancesByDepartmentId/", content);
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var advances = JsonConvert.DeserializeObject<List<AdvanceVM>>(cont);
                return View(advances);

            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateAdvance()
        {
            return View(new AdvancePersonnelVM());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvance(AdvancePersonnelVM model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Advance", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Advances");
            }
            else
            {
                ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdvance(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Advance/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Advances");
            }

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAdvance(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Advance/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var advance = JsonConvert.DeserializeObject<AdvancePersonnelVM>(content);
                return View(advance);

            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdvance(AdvancePersonnelVM model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Advance", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Advances");
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }

        //[HttpGet]
        //public async Task<IActionResult> ApproveAdvance(Guid id)
        //{

        //}
    }
}
