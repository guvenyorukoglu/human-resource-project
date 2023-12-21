using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly HttpClient _httpClient;

        public ExpenseController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Expenses()
        {
            Guid employeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
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

        public async Task<IActionResult> ExpensesDepartment()
        {
            Guid departmentId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "DepartmentId").Value);
            var json = JsonConvert.SerializeObject(departmentId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Expense/GetExpensesByDepartmentId/", content);
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var expenses = JsonConvert.DeserializeObject<List<ExpenseVM>>(cont);
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

        //Expense REQUESTS & CONTROLS
        public async Task<IActionResult> ExpenseRequests()
        {

            var response = await _httpClient.GetAsync($"api/Expense/GetAllExpences/");
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var expenseRequests = JsonConvert.DeserializeObject<List<PersonelVM>>(cont);
                return View(expenseRequests);

            }
            return View();
        }


        [HttpPut]
        public async Task<IActionResult> ExpenseLeave(ExpenseDTO model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Expense/UpdateExpense/{model}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ExpenseRequests");
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> RejectExpense(ExpenseDTO model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Expense/DeleteExpense/{model}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ExpenseRequests");
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }

    }
}
