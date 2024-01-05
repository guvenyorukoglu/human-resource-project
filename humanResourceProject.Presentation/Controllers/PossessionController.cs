using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    [Authorize(Roles = "SiteManager, CompanyManager, Manager, Personel")]
    public class PossessionController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public PossessionController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
        }
        public async Task<IActionResult> Possessions()
        {
            Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);


            var response = await _httpClient.GetAsync($"api/Possession/GetPossessionsByCompanyId/{companyId}");

            if (response.IsSuccessStatusCode)
            {
                var possessionsResponse = await response.Content.ReadAsStringAsync();
                List<PossessionVM>? possessionsList = JsonConvert.DeserializeObject<List<PossessionVM>>(possessionsResponse);

                var json = JsonConvert.SerializeObject(companyId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var responseEmployee = await _httpClient.PostAsync($"api/AppUser/GetEmployeesByCompanyId/", content);
                if (responseEmployee.IsSuccessStatusCode)
                {
                    var employeesContent = await responseEmployee.Content.ReadAsStringAsync();
                    var employees = JsonConvert.DeserializeObject<List<PersonelVM>>(employeesContent);

                    ViewBag.Employees = employees;

                    return View(possessionsList);
                }
            }
            return View("Error");
        }

        [Authorize(Roles = "Manager, Personel")]
        public async Task<IActionResult> MyPossessions()
        {
            Guid employeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var response = await _httpClient.GetAsync($"api/Possession/GetPossessionsByEmployeeId/{employeeId}");

            if (response.IsSuccessStatusCode)
            {
                var possessionsResponse = await response.Content.ReadAsStringAsync();
                List<PersonelPossessionVM>? possessionsList = JsonConvert.DeserializeObject<List<PersonelPossessionVM>>(possessionsResponse);

                return View(possessionsList);
            }
            return View("Error");
        }


        [Authorize(Roles = "CompanyManager")]
        public async Task<IActionResult> CreatePossession()
        {
            Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);

            return View(new PossessionDTO { CompanyId = companyId });

        }

        [Authorize(Roles = "CompanyManager")]
        [HttpPost]
        public async Task<IActionResult> CreatePossession(PossessionDTO model)
        {
            if (!ModelState.IsValid)
            {
                if (ModelState["PossessionType"].Errors.Count > 0)
                {
                    ModelState.AddModelError(string.Empty, "Zimmet türünü seçiniz!");
                }
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Possession/CreatePossession", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["CreatePossessionMessage"] = "Zimmet başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Possessions));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bu barkod numarasına sahip zaten bir zimmet var!");
                return View(model);
            }

        }

        //public async Task<IActionResult> DetailsPossession(Guid id)
        //{
        //    var response = await _httpClient.GetAsync($"api/Possession/DetailsPossession/{id}");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var possessionDetailResponse = await response.Content.ReadAsStringAsync();
        //        PossessionVM? possessionVM = JsonConvert.DeserializeObject<PossessionVM>(possessionDetailResponse);
        //        return View(possessionVM);
        //    }

        //    return View("Error");
        //}

        [Authorize(Roles = "CompanyManager")]
        public async Task<IActionResult> UpdatePossession(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Possession/GetUpdatePossessionDTO/{id}");

            if (response.IsSuccessStatusCode)
            {
                var possessionDTOResponse = await response.Content.ReadAsStringAsync();
                UpdatePossessionDTO? possessionDTO = JsonConvert.DeserializeObject<UpdatePossessionDTO>(possessionDTOResponse);
                return View(possessionDTO);
            }

            return View("Error");
        }

        [Authorize(Roles = "CompanyManager")]
        [HttpPost]
        public async Task<IActionResult> UpdatePossession(UpdatePossessionDTO model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu tekrar deneyiniz!");
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Possession/", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessUpdatePossesionMessage"] = "Zimmet güncellenmiştir.";
                return RedirectToAction(nameof(Possessions));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bu barkod numarasına sahip zaten bir zimmet var!");
                return View(model);
            }
        }

        [Authorize(Roles = "CompanyManager")]
        public async Task<IActionResult> DeletePossession(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Possession/DeletePossession/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Possessions));
            }

            return View("Error");
        }

        [Authorize(Roles = "CompanyManager")]
        [HttpPost]
        public async Task<IActionResult> AssignPossession(Guid employeeId, Guid itemId)
        {
            AssignPossessionDTO assignPossessionDTO = new AssignPossessionDTO
            {
                EmployeeId = employeeId,
                PossessionId = itemId
            };
            var json = JsonConvert.SerializeObject(assignPossessionDTO);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Possession/AssignPossession", content);

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Zimmet atama işleminiz başarılı bir şekilde gerçekleşmiştir." });
            }
            return Json(new { success = false, message = "Zimmet atama işleminiz başarısız." });
        }

        [Authorize(Roles = "CompanyManager")]
        public async Task<IActionResult> TakeBackPossession(Guid id)
        {
            var json = JsonConvert.SerializeObject(id);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Possession/TakeBackPossession/", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Possessions));
            }

            return View("Error");
        }
    }
}
