using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    [Authorize(Roles = "SiteManager, CompanyManager")]
    public class DepartmentController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
        }
        public async Task<IActionResult> Departments()
        {
            Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);

            var response = await _httpClient.GetAsync($"api/Department/GetDepartmentsByCompanyId/{companyId}");

            if (response.IsSuccessStatusCode)
            {
                var departmentsResponse = await response.Content.ReadAsStringAsync();
                List<DepartmentVM>? departmentsList = JsonConvert.DeserializeObject<List<DepartmentVM>>(departmentsResponse);
                return View(departmentsList);
            }

            return View("Error");
        }

        public async Task<IActionResult> CreateDepartment()
        {
            Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);

            return View(new DepartmentDTO { CompanyId = companyId });

        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(DepartmentDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Department/CreateDepartment", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessCreateDepartmentMessage"] = "Şirketinize Departman Eklenmiştir.";
                return RedirectToAction(nameof(Departments));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bu departman adına sahip zaten bir departman var!");
                return View(model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> UpdateDepartment(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Department/GetUpdateDepartmentDTO/{id}");

            if (response.IsSuccessStatusCode)
            {
                var departmentDTOResponse = await response.Content.ReadAsStringAsync();
                UpdateDepartmentDTO? departmentDTO = JsonConvert.DeserializeObject<UpdateDepartmentDTO>(departmentDTOResponse);
                return View(departmentDTO);
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDepartment(UpdateDepartmentDTO model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu tekrar deneyiniz!");
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Department/", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessUpdateDepartmentMessage"] = "Departman tanımı güncellenmiştir.";
                return RedirectToAction(nameof(Departments));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bu departman adına sahip zaten bir departman var!");
                return View(model);
            }
        }

        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Department/DeleteDepartment/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Departments));
            }

            return View("Error");
        }
    }
}
