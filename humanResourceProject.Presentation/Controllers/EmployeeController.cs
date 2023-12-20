using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    [Authorize(Roles = "CompanyManager, DepartmentManager")]
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://monitoreaseapi.azurewebsites.net"); // Azure
            _httpClient.BaseAddress = new Uri("https://localhost:7255/");
        }

        public async Task<IActionResult> Employees()
        {
            if (User.IsInRole("CompanyManager")) // Şirket Yöneticisi ise tüm personelleri getirir
            {
                Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);
                var json = JsonConvert.SerializeObject(companyId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"api/AppUser/GetEmployeesByCompanyId/", content);
                if (response.IsSuccessStatusCode)
                {
                    var cont = await response.Content.ReadAsStringAsync();
                    var employees = JsonConvert.DeserializeObject<List<PersonelVM>>(cont);
                    return View(employees);

                }
                return View();
            }
            else if (User.IsInRole("DepartmentManager")) // Departman Yöneticisi ise departmanındaki personelleri getirir
            {
                Guid departmentId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "DepartmentId").Value);
                var json = JsonConvert.SerializeObject(departmentId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"api/AppUser/GetEmployeesByDepartmentId/", content);
                if (response.IsSuccessStatusCode)
                {
                    var cont = await response.Content.ReadAsStringAsync();
                    var employees = JsonConvert.DeserializeObject<List<PersonelVM>>(cont);
                    return View(employees);

                }
                return View();
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult CreatePersonel()
        {
            if (User.IsInRole("CompanyManager"))
                return View(new CreateEmployeeDTO());
            else if(User.IsInRole("DepartmentManager"))
                return View(new CreateEmployeeDTO()
                {
                    DepartmentId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "DepartmentId").Value),
                    ManagerId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id").Value)
                });
            else
                return View();
        }

        [HttpGet]
        public IActionResult CreatePersonelManager()
        {
            return View(new CreateEmployeeDTO()
            {
                ManagerId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id").Value)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonel(UserRegisterDTO model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/AppUser", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Employees");
            }
            else
            {
                ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonelManager(UserRegisterDTO model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/AppUser", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Employees");
            }
            else
            {
                ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/AppUser/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Employees");
            }

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/AppUser/GetUpdateUserDTO/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var employee = JsonConvert.DeserializeObject<UpdateUserDTO>(content);
                return View(employee);

            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(UpdateUserDTO employee)
        {
            var json = JsonConvert.SerializeObject(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/AppUser", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Employees");
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(employee);
        }


        public async Task<IActionResult> Home()
        {
            return View();
        }

    }
}
