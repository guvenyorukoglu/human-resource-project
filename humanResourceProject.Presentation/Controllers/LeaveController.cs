using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    public class LeaveController : Controller
    {
        private readonly HttpClient _httpClient;

        public LeaveController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Leaves()
        {
            Guid employeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var json = JsonConvert.SerializeObject(employeeId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Leave/GetLeavesByEmployeeId/", content);
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var leaves = JsonConvert.DeserializeObject<List<LeavePersonnelVM>>(cont);
                return View(leaves);

            }
            return View();
        }

        public async Task<IActionResult> LeavesDepartment()
        {
            Guid departmentId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "DepartmentId").Value);
            var json = JsonConvert.SerializeObject(departmentId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Expense/GetLeavesByDepartmentId/", content);
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var leaves = JsonConvert.DeserializeObject<List<LeaveVM>>(cont);
                return View(leaves);

            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateLeave()
        {
            return View(new LeavePersonnelVM());
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeave(LeavePersonnelVM model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Leave", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Leave");
            }
            else
            {
                ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLeave(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Leave/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Leave");
            }

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateLeave(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Leave/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var leave = JsonConvert.DeserializeObject<LeavePersonnelVM>(content);
                return View(leave);

            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLeave(LeavePersonnelVM model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Leave", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Leave");
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
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
    }
}

