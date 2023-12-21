using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    [Authorize]
    public class AdvanceController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdvanceController()
        {
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://monitoreaseapi.azurewebsites.net"); // Azure
            _httpClient.BaseAddress = new Uri("https://localhost:7255/"); // Local
        }

        [Authorize(Roles = "DepartmentManager,Personel")]
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

        [Authorize(Roles = "DepartmentManager,CompanyManager")]
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
            return View(new AdvanceDTO()
            {
                EmployeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvance(AdvanceDTO model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Advance", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(MyAdvances));
            }
            else
            {
                ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAdvance(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Advance/DeleteAdvance/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(MyAdvances));
            }

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAdvance(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Advance/GetUpdateAdvanceDTO/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var updateAdvanceDTO = JsonConvert.DeserializeObject<UpdateAdvanceDTO>(content);
                return View(updateAdvanceDTO);

            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdvance(UpdateAdvanceDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Advance", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(MyAdvances));
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }


        //ADVANCE REQUESTS & CONTROLS
        
        [Authorize(Roles = "DepartmentManager,CompanyManager")]
        [HttpGet]
        public async Task<IActionResult> ApproveAdvance(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Advance/GetUpdateAdvanceDTO/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var content = await response.Content.ReadAsStringAsync();
            var updateAdvanceDTO = JsonConvert.DeserializeObject<UpdateAdvanceDTO>(content);
            updateAdvanceDTO.AdvanceStatus = RequestStatus.Approved;

            var json = JsonConvert.SerializeObject(updateAdvanceDTO);
            var contentDTO = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PutAsync($"api/Advance/UpdateStatus", contentDTO);

            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(EmployeesAdvances));
            }

            ModelState.AddModelError(httpResponse.StatusCode.ToString(), "Bir hata oluştu.");
            return View("Error");
        }

        [Authorize(Roles = "DepartmentManager,CompanyManager")]
        [HttpGet]
        public async Task<IActionResult> RejectAdvance(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Advance/GetUpdateAdvanceDTO/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var content = await response.Content.ReadAsStringAsync();
            var updateAdvanceDTO = JsonConvert.DeserializeObject<UpdateAdvanceDTO>(content);
            updateAdvanceDTO.AdvanceStatus = RequestStatus.Rejected;

            var json = JsonConvert.SerializeObject(updateAdvanceDTO);
            var contentDTO = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PutAsync($"api/Advance/UpdateStatus/", contentDTO);

            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(EmployeesAdvances));
            }

            ModelState.AddModelError(httpResponse.StatusCode.ToString(), "Bir hata oluştu.");
            return View("Error");
        }

    }
}
