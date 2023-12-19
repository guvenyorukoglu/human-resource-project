using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    public class PersonnelManagerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PersonnelManagerController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://monitoreaseapi.azurewebsites.net"); // Azure
            _httpClient.BaseAddress = new Uri("https://localhost:7255/");
        }


        public async Task<IActionResult> Home()
        {
            return View();
        }

        //LEAVE REQUESTS & CONTROLS
        public async Task<IActionResult> LeaveRequests()
        {

            var response = await _httpClient.GetAsync($"api/Leave/GetAllLeaves/");
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var leaveRequests = JsonConvert.DeserializeObject<List<PersonelVM>>(cont);
                return View(leaveRequests);

            }
            return View();
        }


        [HttpPut]
        public async Task<IActionResult> ApproveLeave(LeaveDTO model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Leave/UpdateLeave/{model}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("LeaveRequests");
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> RejectLeave(LeaveDTO model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Leave/DeleteLeave/{model}", content);

            if (response.IsSuccessStatusCode)
        {
                return RedirectToAction("LeaveRequests");
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
