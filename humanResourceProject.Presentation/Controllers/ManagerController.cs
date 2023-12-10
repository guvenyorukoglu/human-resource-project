using humanResourceProject.Domain.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace humanResourceProject.Presentation.Controllers
{
    public class ManagerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ManagerController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7255/");

        }


        public async Task<IActionResult> Employees()
        {
                   
           
            var response = await _httpClient.GetAsync("api/AppUser");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var employees = JsonConvert.DeserializeObject<List<AppUser>>(content);
                return View(employees);

            }
            return View("Error");
        }

    }
}
