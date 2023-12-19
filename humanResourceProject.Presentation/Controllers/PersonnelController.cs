using humanResourceProject.Models.DTOs;
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

        [HttpGet]
        public IActionResult CreateAdvance()
        {
            return View(new AdvanceDTO());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvance(AdvanceDTO model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Personnel", content);
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

        [HttpGet]
        public async Task<IActionResult> DeleteAdvance(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Personnel/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Advances");
            }

            return View("Error");
        }


    }
}
