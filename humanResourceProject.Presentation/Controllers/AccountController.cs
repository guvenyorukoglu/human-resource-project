using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace humanResourceProject.Presentation.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://monitoreaseapi.azurewebsites.net"); // Azure
            _httpClient.BaseAddress = new Uri("https://localhost:7255/"); // Local
        }

        [AllowAnonymous]
        public IActionResult RegisterCompany() // İlk açılacak register view'i (şirket kaydı için)
        {
            return View(new CompanyRegisterDTO());
        }

        [AllowAnonymous]
        public IActionResult Register(Guid companyId) // İleri butonuna basıldığında açılacak şirket yöneticisi register view'i
        {
            UserRegisterDTO userRegisterDTO = new UserRegisterDTO();
            userRegisterDTO.CompanyId = companyId;
            return View(userRegisterDTO);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterCompany(CompanyRegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/api/Account/RegisterCompany", content);

            if (response.IsSuccessStatusCode)
            {
                var companyId = await response.Content.ReadAsStringAsync();
                companyId = companyId.Replace("\"", "");
                Guid id = Guid.Parse(companyId);
                return RedirectToAction("Register", new { companyId = id });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu. Tekrar deneyiniz!");
                return View(model);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Model valid değil ise validation errorları ile birlikte register sayfasına geri döner
            }

            // Serialize the model to JSON
            var json = JsonConvert.SerializeObject(model);

            // Create a StringContent from the serialized JSON
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send a POST request to the API endpoint to create the resource
            HttpResponseMessage response = await _httpClient.PostAsync("/api/Account/RegisterUser", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu. Tekrar deneyiniz!");
                return View(model);
            }
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            returnUrl = returnUrl is null ? "/Home/Index" : returnUrl;
            return View(new LoginDTO() { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var json = System.Text.Json.JsonSerializer.Serialize(model);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/api/account/login", content);

            if (response.IsSuccessStatusCode)
            {
                var api_response = await response.Content.ReadAsStringAsync();
                dynamic parsedResponse = JsonConvert.DeserializeObject(api_response);
                string userId = parsedResponse.userId;
                string name = parsedResponse.name;
                string surname = parsedResponse.surname;
                string companyId = parsedResponse.companyId;

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Email, model.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
                claims.Add(new Claim(ClaimTypes.Name, name));
                claims.Add(new Claim(ClaimTypes.Surname, surname));
                claims.Add(new Claim("CompanyId", companyId));

    
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return Redirect(model.ReturnUrl ?? "/");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login işlemi başarısız. Tekrar deneyin...");
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/Account/Logout");

            if (response.IsSuccessStatusCode)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
