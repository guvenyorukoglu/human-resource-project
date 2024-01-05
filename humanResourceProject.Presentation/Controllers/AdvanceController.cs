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
        private readonly IConfiguration _configuration;

        public AdvanceController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
        }

        [Authorize(Roles = "Manager,Personel")]
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

        [Authorize(Roles = "Manager,CompanyManager")]
        public async Task<IActionResult> EmployeesAdvances()
        {
            if (User.IsInRole("Manager") || User.IsInRole("CompanyManager"))
            {
                Guid managerId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

                var response = await _httpClient.GetAsync($"api/Advance/GetAdvancesByManagerId/{managerId}");
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
        public async Task<IActionResult> CreateAdvance(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Advance/GetAdvanceDTO/{id}");
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var advanceDTO = JsonConvert.DeserializeObject<AdvanceDTO>(cont);
                return View(advanceDTO);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvance(AdvanceDTO model)
        {
            if (model.ExpiryDate < DateTime.Now)
            {
                ModelState.AddModelError("ExpiryDate", "Tarih bugünden önceki bir tarih olmamalıdır.");
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                if (ModelState["AdvanceType"].Errors.Count > 0)
                {
                    ModelState.AddModelError(string.Empty, "Avans türünü seçiniz!");
                }
                if (ModelState["Currency"].Errors.Count > 0)
                {
                    ModelState.AddModelError(string.Empty, "Para birimini seçiniz!");
                }
                return View(model);
            }

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

            var response = await _httpClient.PutAsync($"api/Advance/", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(MyAdvances));
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }


        //ADVANCE REQUESTS & CONTROLS

        [Authorize(Roles = "Manager,CompanyManager")]
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

        [Authorize(Roles = "Manager,CompanyManager")]
        [HttpPost]
        public async Task<IActionResult> RejectAdvance(Guid id, string rejectReason)
        {
            var response = await _httpClient.GetAsync($"api/Advance/GetUpdateAdvanceDTO/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var content = await response.Content.ReadAsStringAsync();
            var updateAdvanceDTO = JsonConvert.DeserializeObject<UpdateAdvanceDTO>(content);
            updateAdvanceDTO.AdvanceStatus = RequestStatus.Rejected;
            updateAdvanceDTO.RejectReason = rejectReason;

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
