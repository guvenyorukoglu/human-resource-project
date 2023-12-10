using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace humanResourceProject.Presentation.Controllers
{
    public class AccountController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7255/");

        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDTO model) // VM kullanılabilir
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Model valid değil ise validation errorları ile birlikte register sayfasına geri döner
            }

            // Serialize the model to JSON
            var json = JsonSerializer.Serialize(model);

            // Create a StringContent from the serialized JSON
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send a POST request to the API endpoint to create the resource
            HttpResponseMessage response = await _httpClient.PostAsync("/api/Account/Register", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to create the resource. Please try again.");
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

            var json = JsonSerializer.Serialize(model);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/api/account/login", content);

            if (response.IsSuccessStatusCode)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Email, model.Email));

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return Redirect(model.ReturnUrl ?? "/");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to create the resource. Please try again.");
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/Account/Logout");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return BadRequest();
            }
        }


        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
