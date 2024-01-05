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
    public class LeaveController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public LeaveController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
        }

        [Authorize(Roles = "Manager,Personel")]
        public async Task<IActionResult> MyLeaves()
        {
            Guid employeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _httpClient.GetAsync($"api/Leave/GetLeavesByEmployeeId/{employeeId}");
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var leaves = JsonConvert.DeserializeObject<List<LeavePersonnelVM>>(cont);
                return View(leaves);
            }
            return View();
        }

        [Authorize(Roles = "Manager,CompanyManager")]
        public async Task<IActionResult> EmployeesLeaves()
        {
            if(User.IsInRole("Manager") || User.IsInRole("CompanyManager"))
            {
                Guid managerId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

                var response = await _httpClient.GetAsync($"api/Leave/GetLeavesByManagerId/{managerId}");
                if (response.IsSuccessStatusCode)
                {
                    var cont = await response.Content.ReadAsStringAsync();
                    var leaves = JsonConvert.DeserializeObject<List<LeaveVM>>(cont);
                    return View(leaves);
                }
                return View();
            }
            //else if(User.IsInRole("CompanyManager"))
            //{
            //    Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);

            //    var response = await _httpClient.GetAsync($"api/Leave/GetLeavesByCompanyId/{companyId}");
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var cont = await response.Content.ReadAsStringAsync();
            //        var leaves = JsonConvert.DeserializeObject<List<LeaveVM>>(cont);
            //        return View(leaves);
            //    }
            //    return View();
            //}
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateLeave(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Leave/GetLeaveDTO/{id}");
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var leaveDTO = JsonConvert.DeserializeObject<LeaveDTO>(cont);
                return View(leaveDTO);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeave(LeaveDTO model)
        {

            if (!ModelState.IsValid)
            {
                if (ModelState["LeaveType"].Errors.Count > 0)
                {
                    ModelState.AddModelError(string.Empty, "İzin türünü seçiniz!");
                }
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Leave", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(MyLeaves));

            else
            {
                ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteLeave(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Leave/DeleteLeave/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(MyLeaves));

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateLeave(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Leave/GetUpdateLeaveDTO/{id}");

            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var updateLeaveDTO = JsonConvert.DeserializeObject<UpdateLeaveDTO>(cont);
                return View(updateLeaveDTO);
            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLeave(UpdateLeaveDTO model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu tekrar deneyiniz!");
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Leave", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessUpdateLeaveMessage"] = "İzin talebiniz güncellenmiştir.";
                return RedirectToAction(nameof(MyLeaves));
            }
            else
            {
                ModelState.AddModelError("ModelInvalid", "Girilen bilgileri kontrol edin. Güncelleme başarısız!");
                return View(model);
            }
        }

        //LEAVE REQUESTS & CONTROLS
        [Authorize(Roles = "Manager,CompanyManager")]
        [HttpGet]
        public async Task<IActionResult> ApproveLeave(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Leave/GetUpdateLeaveDTO/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var content = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<UpdateLeaveDTO>(content);
            model.LeaveStatus = RequestStatus.Approved;

            var json = JsonConvert.SerializeObject(model);
            var contentDTO = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PutAsync($"api/Leave/UpdateStatus", contentDTO);

            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(EmployeesLeaves));
            }
            ModelState.AddModelError(httpResponse.StatusCode.ToString(), "Bir hata oluştu.");
            return View("Error");
        }

        [Authorize(Roles = "Manager,CompanyManager")]
        [HttpPost]
        public async Task<IActionResult> RejectLeave(Guid id, string rejectReason)
        {
            var response = await _httpClient.GetAsync($"api/Leave/GetUpdateLeaveDTO/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");
            
            var content = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<UpdateLeaveDTO>(content);
            model.LeaveStatus = RequestStatus.Rejected;
            model.RejectReason = rejectReason;

            var json = JsonConvert.SerializeObject(model);
            var contentDTO = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PutAsync($"api/Leave/UpdateStatus", contentDTO);

            if (httpResponse.IsSuccessStatusCode)
                return RedirectToAction(nameof(EmployeesLeaves));

            ModelState.AddModelError(httpResponse.StatusCode.ToString(), "Bir hata oluştu.");
            return View("Error");
        }
    }
}

