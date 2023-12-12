using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

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
            _httpClient.BaseAddress = new Uri("https://localhost:7255/");

        }

        public async Task<IActionResult> Companies()
        {

            HttpResponseMessage response = await _httpClient.GetAsync("/api/Company");

            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();
                var companies = JsonSerializer.Deserialize<List<CompanyVM>>(json);

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
                ModelState.AddModelError(string.Empty, "Failed to create the resource. Please try again.");
                return View(model);
            }
        }

        //[HttpGet]
        //public IActionResult UpdateCompany(Guid id)
        //{
        //    var company = _companyReadService.GetById(id);

        //    if (company == null)
        //    {
        //        return NotFound();
        //    }

        //    var companyUpdateDTO = new UpdateCompanyDTO
        //    {
        //        CompanyName = company.CompanyName,
        //        Adress = company.Address,
        //        PhoneNumber = company.PhoneNumber,
        //        NumberOfEmployees = company.NumberOfEmployees
        //    };

        //    return View(companyUpdateDTO);
        //}



        //[HttpPost]
        //public async Task<IActionResult> UpdateCompany(UpdateCompanyDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var json = JsonSerializer.Serialize(model);

        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await _httpClient.PostAsync("/api/Company/Update", content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction(nameof(Companies));
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Failed to update the resource. Please try again.");
        //            return View("UpdateCompany", model);
        //        }
        //    }


        //    return View("UpdateCompany", model);
        //}



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
