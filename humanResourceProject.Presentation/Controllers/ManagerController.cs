 using Bogus.Bson;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

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

            Guid id = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var res = await _httpClient.GetAsync($"api/AppUser/{id}");
            if (res.IsSuccessStatusCode)

            {
                var content = await res.Content.ReadAsStringAsync();
                var employee = JsonConvert.DeserializeObject<AppUser>(content);




                var response = await _httpClient.GetAsync($"api/AppUser/GetEmployeesByCompanyId/{employee.CompanyId}");
                if (response.IsSuccessStatusCode)
                {
                    var cont = await response.Content.ReadAsStringAsync();
                    var employees = JsonConvert.DeserializeObject<List<PersonelVM>>(cont);
                    return View(employees);

                }
            }
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            return Ok();
        }



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
        public async Task<IActionResult> EditEmployee(UpdateUserDTO updateUserDTO)
        {

            var json = JsonConvert.SerializeObject(updateUserDTO);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/AppUser", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Employees");
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(updateUserDTO);
        }



    }
}
