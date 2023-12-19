using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    public class PersonnelController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PersonnelController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://monitoreaseapi.azurewebsites.net"); // Azure
            _httpClient.BaseAddress = new Uri("https://localhost:7255/");
        }
        public IActionResult Home()
        {
            return View();
        }

        public async Task<IActionResult> Advances()
        {
            Guid employeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id").Value);
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

        [HttpGet]
        public IActionResult CreateAdvance()
        {
            return View(new AdvancePersonnelVM());
        }

        [HttpPost]
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

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

        public async Task<IActionResult> DeleteAdvance(Guid id)
        {

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Advances");
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }


        public async Task<IActionResult> Expenses()
        {
            Guid employeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id").Value);
            var json = JsonConvert.SerializeObject(employeeId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Expense/GetExpensesByEmployeeId/", content);
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var expenses = JsonConvert.DeserializeObject<List<ExpensePersonnelVM>>(cont);
                return View(expenses);

            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateExpense()
        {
            return View(new ExpensePersonnelVM());
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense(ExpensePersonnelVM model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Expense", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Expense");
            }
            else
            {
                ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Expense/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Expense");
            }

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateExpense(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Expense/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var expense = JsonConvert.DeserializeObject<ExpensePersonnelVM>(content);
                return View(expense);

            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateExpense(ExpensePersonnelVM model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Expense", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Expense");
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }


        public async Task<IActionResult> Leaves()
        {
            Guid employeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id").Value);
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
    }
}
