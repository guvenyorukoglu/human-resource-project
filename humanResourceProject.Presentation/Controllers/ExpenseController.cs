using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly HttpClient _httpClient;

        public ExpenseController()
        {
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://monitoreaseapi.azurewebsites.net"); // Azure
            _httpClient.BaseAddress = new Uri("https://localhost:7255/"); // Local
        }

        [Authorize(Roles = "Manager,Personel")]
        public async Task<IActionResult> MyExpenses()
        {
            Guid employeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _httpClient.GetAsync($"api/Expense/GetExpensesByEmployeeId/{employeeId}");
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var expenses = JsonConvert.DeserializeObject<List<ExpensePersonnelVM>>(cont);
                return View(expenses);

            }
            return View();
        }

        [Authorize(Roles = "Manager,CompanyManager")]
        public async Task<IActionResult> EmployeesExpenses()
        {
            if (User.IsInRole("Manager"))
            {
                Guid managerId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var response = await _httpClient.GetAsync($"api/Expense/GetExpensesByManagerId/{managerId}");
                if (response.IsSuccessStatusCode)
                {
                    var cont = await response.Content.ReadAsStringAsync();
                    var expenses = JsonConvert.DeserializeObject<List<ExpenseVM>>(cont);
                    return View(expenses);
                }
                return View();
            }
            else if (User.IsInRole("CompanyManager"))
            {
                Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);

                var response = await _httpClient.GetAsync($"api/Expense/GetExpensesByCompanyId/{companyId}");
                if (response.IsSuccessStatusCode)
                {
                    var cont = await response.Content.ReadAsStringAsync();
                    var expenses = JsonConvert.DeserializeObject<List<ExpenseVM>>(cont);
                    return View(expenses);
                }
                return View();
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateExpense(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Expense/GetExpenseDTO/{id}");
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var expenseDTO = JsonConvert.DeserializeObject<ExpenseDTO>(cont);
                return View(expenseDTO);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense(ExpenseDTO model)
        {

            if (model.DateOfExpense.Date>DateTime.Now.Date)
            {
                ModelState.AddModelError("DateOfExpense", "Tarih bugünden sonraki bir tarih olmamalıdır.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                if (ModelState["ExpenseType"].Errors.Count > 0)
                {
                    ModelState.AddModelError(string.Empty, "Masraf türünü seçiniz!");
                }
                if (ModelState["Currency"].Errors.Count > 0)
                {
                    ModelState.AddModelError(string.Empty, "Para birimini seçiniz!");
                }
                return View(model);
            }

            var multipartContent = new MultipartFormDataContent();

            var properties = typeof(ExpenseDTO).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(model)?.ToString() ?? string.Empty;
                var stringContent = new StringContent(value, Encoding.UTF8, MediaTypeNames.Text.Plain);
                multipartContent.Add(stringContent, property.Name);
            }

            if (model.UploadPath != null && model.UploadPath.Length > 0) // UploadPath varsa
            {
                string fileExtension = Path.GetExtension(model.UploadPath.FileName).ToLower();

                if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".jpeg")
                {
                    ModelState.AddModelError(string.Empty, "Yüklediğiniz profil fotoğrafının uzantısı '.png', '.jpg' veya '.jpeg' olmalıdır.");
                    return View(model);
                }

                var imageContent = new StreamContent(model.UploadPath.OpenReadStream());
                multipartContent.Add(imageContent, "UploadPath", model.UploadPath.FileName);
            }

            var response = await _httpClient.PostAsync($"api/Expense/CreateExpense", multipartContent);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(MyExpenses));
            else
            {
                ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Expense/DeleteExpense/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(MyExpenses));

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateExpense(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Expense/GetUpdateExpenseDTO/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var expense = JsonConvert.DeserializeObject<UpdateExpenseDTO>(content);
                return View(expense);

            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateExpense(UpdateExpenseDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var multipartContent = new MultipartFormDataContent();

            var properties = typeof(UpdateExpenseDTO).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(model)?.ToString() ?? string.Empty;
                var stringContent = new StringContent(value, Encoding.UTF8, MediaTypeNames.Text.Plain);
                multipartContent.Add(stringContent, property.Name);
            }

            if (model.UploadPath != null && model.UploadPath.Length > 0)
            {
                string fileExtension = Path.GetExtension(model.UploadPath.FileName).ToLower();

                if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".jpeg")
                {
                    ModelState.AddModelError(string.Empty, "Yüklediğiniz profil fotoğrafının uzantısı '.png', '.jpg' veya '.jpeg' olmalıdır.");
                    return View(model);
                }

                var imageContent = new StreamContent(model.UploadPath.OpenReadStream());
                multipartContent.Add(imageContent, "UploadPath", model.UploadPath.FileName);
            }

            //var json = JsonConvert.SerializeObject(model);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Expense", multipartContent);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(MyExpenses));

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }

        //Expense REQUESTS & CONTROLS
        [Authorize(Roles = "Manager,CompanyManager")]
        [HttpGet]
        public async Task<IActionResult> ApproveExpense(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Expense/GetUpdateExpenseDTO/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var content = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<UpdateExpenseDTO>(content);
            model.ExpenseStatus = RequestStatus.Approved;

            var json = JsonConvert.SerializeObject(model);
            var contentDTO = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PutAsync($"api/Expense/UpdateStatus", contentDTO);

            if (httpResponse.IsSuccessStatusCode)
                return RedirectToAction(nameof(EmployeesExpenses));

            ModelState.AddModelError(httpResponse.StatusCode.ToString(), "Bir hata oluştu.");
            return View("Error");
        }

        [Authorize(Roles = "Manager,CompanyManager")]
        [HttpPost]
        public async Task<IActionResult> RejectExpense(Guid id, string rejectReason)
        {
            var response = await _httpClient.GetAsync($"api/Expense/GetUpdateExpenseDTO/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var content = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<UpdateExpenseDTO>(content);
            model.ExpenseStatus = RequestStatus.Rejected;
            model.RejectReason = rejectReason;

            var json = JsonConvert.SerializeObject(model);
            var contentDTO = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PutAsync($"api/Expense/UpdateStatus", contentDTO);

            if (httpResponse.IsSuccessStatusCode)
                return RedirectToAction(nameof(EmployeesExpenses));

            ModelState.AddModelError(httpResponse.StatusCode.ToString(), "Bir hata oluştu.");
            return View("Error");
        }
    }
}
