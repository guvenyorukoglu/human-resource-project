using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    public class AdvanceController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdvanceController()
        {
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://monitoreaseapi.azurewebsites.net"); // Azure
            _httpClient.BaseAddress = new Uri("https://localhost:7255/"); // Local
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyAdvances()
        {
            Guid employeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _httpClient.GetAsync($"api/Advance/GetAdvancesByEmployeeId/{employeeId}");
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var advances = JsonConvert.DeserializeObject<List<AdvancePersonnelVM>>(cont);
                return View(advances);

            }
            return View();
        }


        public async Task<IActionResult> EmployeesAdvances()
        {
            if (User.IsInRole("DepartmentManager"))
            {
                Guid depatmentId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "DepartmentId").Value);

                var response = await _httpClient.GetAsync($"api/Advance/GetAdvancesByDepartmentId/{depatmentId}");
                if (response.IsSuccessStatusCode)
                {
                    var cont = await response.Content.ReadAsStringAsync();
                    var advances = JsonConvert.DeserializeObject<List<AdvanceVM>>(cont);
                    return View(advances);
                }
                return View();
            }
            else if (User.IsInRole("CompanyManager"))
            {
                Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);

                var response = await _httpClient.GetAsync($"api/Advance/GetAdvancesByCompanyId/{companyId}");
                if (response.IsSuccessStatusCode)
                {
                    var cont = await response.Content.ReadAsStringAsync();
                    var advances = JsonConvert.DeserializeObject<List<AdvanceVM>>(cont);
                    return View(advances);
                }
                return View();
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateAdvance()
        {
            return View(new AdvanceDTO());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvance(AdvanceDTO model)
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


        //ADVANCE REQUESTS & CONTROLS
        public async Task<IActionResult> AdvanceRequests()
        {
            var response = await _httpClient.GetAsync($"api/Leave/GetAllAdvances");
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var leaveRequests = JsonConvert.DeserializeObject<List<PersonelVM>>(cont);
                return View(leaveRequests);

            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ApproveAdvance(AdvanceDTO model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Advance/UpdateAdvance/{model}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AdvanceRequests");
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RejectAdvance(AdvanceDTO model)
        {

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Advance/DeleteAdvance/{model}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AdvanceRequests");
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }

    }
}
