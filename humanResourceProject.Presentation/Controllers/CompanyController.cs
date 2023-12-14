using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace humanResourceProject.Presentation.Controllers
{
    [Authorize(Roles = "SiteManager")]
    public class CompanyController : Controller
    {
        private readonly ICompanyReadService _companyReadService;
        private readonly ICompanyWriteService _companyWriteService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CompanyController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://monitoreaseapi.azurewebsites.net"); // Azure
            _httpClient.BaseAddress = new Uri("https://localhost:7255/");

        }

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




        public IActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost]
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
        public async Task<IActionResult> UpdateCompany(Guid id)
        {

            var response = await _httpClient.GetAsync($"api/Company/GetUpdateCompanyDTO/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var company = JsonConvert.DeserializeObject<UpdateCompanyDTO>(content);
                return View(company);

            }
            return View("Error");


        }



        [HttpPost]
        public async Task<IActionResult> UpdateCompany(UpdateCompanyDTO model)
        {


            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Company", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Companies");
            }


            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/Company/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Silme işlemi başarısız olursa, hata mesajını ModelState'e ekleyebilirsiniz.
                ModelState.AddModelError(string.Empty, "Failed to delete the resource. Please try again.");

                // Hata durumunda, mevcut sayfada kalabilir veya başka bir sayfaya yönlendirebilirsiniz.
                HttpResponseMessage getResponse = await _httpClient.GetAsync($"/api/Company/{id}");

                if (getResponse.IsSuccessStatusCode)
                {
                    var json = await getResponse.Content.ReadAsStringAsync();
                    var company = JsonSerializer.Deserialize<CompanyVM>(json);

                    return View("Delete", company);
                }
                else
                {
                    return NotFound();
                }
            }
        }



    }
}
