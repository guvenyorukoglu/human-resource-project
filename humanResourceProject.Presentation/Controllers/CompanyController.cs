using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace humanResourceProject.Presentation.Controllers
{
    [AllowAnonymous]
    public class CompanyController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
      
        public CompanyController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
        }
        [Authorize(Roles = "SiteManager")]
        public async Task<IActionResult> Companies()
        {

            HttpResponseMessage response = await _httpClient.GetAsync("/api/Company");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var companies = JsonConvert.DeserializeObject<List<CompanyVM>>(json);

                return View(companies);
            }
            else
            {
                return NotFound();
            }
        }



        [AllowAnonymous]
        public IActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCompany(CompanyRegisterDTO model)
        {


            if (!ModelState.IsValid)
            {
                return View(model); // Model valid değil ise validation errorları ile birlikte register sayfasına geri döner
            }

            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/api/Company/Create", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Şirket Oluşturulamadı");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "CompanyManager")]
        public async Task<IActionResult> UpdateCompany(Guid id)
        {

            var response = await _httpClient.GetAsync($"api/Company/GetUpdateCompanyDTO/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<UpdateCompanyDTO>(content);

                return View(model);
            }
            return View("Error");

        }

        [HttpPost]
        [Authorize(Roles = "CompanyManager")]
        public async Task<IActionResult> UpdateCompany(UpdateCompanyDTO model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu tekrar deneyiniz!");
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Company", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessUpdateCompanyMessage"] = "Şirket bilgileri güncellenmiştir.";
                return RedirectToAction(nameof(Companies));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu!");
                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SiteManager")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/Company/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to delete the resource. Please try again.");

                HttpResponseMessage getResponse = await _httpClient.GetAsync($"/api/Company/{id}");

                if (getResponse.IsSuccessStatusCode)
                {
                    var json = await getResponse.Content.ReadAsStringAsync();
                    var company = JsonSerializer.Deserialize<CompanyVM>(json);

                    return RedirectToAction(nameof(Companies));
                }
                else
                {
                    return NotFound();
                }
            }
        }
        
        [HttpGet]
        [Authorize(Roles = "SiteManager")]
        public async Task<IActionResult> ApproveCompany(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Company/GetUpdateCompanyDTO/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var content = await response.Content.ReadAsStringAsync();
            var model2 = JsonConvert.DeserializeObject<UpdateCompanyDTO>(content);
            model2.CompanyStatus = RequestStatus.Approved;
            model2.Status = Status.Active;
            var json = JsonConvert.SerializeObject(model2);
            var model = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PutAsync($"api/Company/UpdateStatus", model);

            if (httpResponse.IsSuccessStatusCode)
                return RedirectToAction(nameof(Companies));

            ModelState.AddModelError(httpResponse.StatusCode.ToString(), "Bir hata oluştu.");
            return View("Error");
        }

        [Authorize(Roles = "SiteManager")]
        [HttpPost]
        public async Task<IActionResult> RejectCompany(Guid id, string rejectReason)
        {
            var response = await _httpClient.GetAsync($"api/Company/GetUpdateCompanyDTO/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var content = await response.Content.ReadAsStringAsync();
            var model2 = JsonConvert.DeserializeObject<UpdateCompanyDTO>(content);
            model2.RejectReason = rejectReason;
            model2.CompanyStatus = RequestStatus.Rejected;
            var json = JsonConvert.SerializeObject(model2);
            var model = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PutAsync($"api/Company/UpdateStatus", model);

            if (httpResponse.IsSuccessStatusCode)
                return RedirectToAction(nameof(Companies));

            ModelState.AddModelError(httpResponse.StatusCode.ToString(), "Bir hata oluştu.");
            return View("Error");
        }



    }
}
