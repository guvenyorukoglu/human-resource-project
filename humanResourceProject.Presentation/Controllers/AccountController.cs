using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        List<JobVM> jobs;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://monitoreaseapi.azurewebsites.net"); // Azure
            _httpClient.BaseAddress = new Uri("https://localhost:7255/"); // Local
            jobs = new List<JobVM>();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterCompany() // İlk açılacak register view'i (şirket kaydı için)
        {
            return View(new CompanyRegisterDTO());
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Guid companyId) // İleri butonuna basıldığında açılacak şirket yöneticisi register view'i
        {
            UserRegisterDTO userRegisterDTO = new UserRegisterDTO();
            userRegisterDTO.CompanyId = companyId;
            userRegisterDTO.JobId = Guid.Empty;
            userRegisterDTO.DepartmentId = Guid.Empty;

            return View(userRegisterDTO);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterCompany(CompanyRegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Result"] = "modelinvalid";
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
                TempData["Result"] = "success";
                return RedirectToAction("Register", new { companyId = id });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu. Tekrar deneyiniz!");
                TempData["Result"] = "error";
                return View(model);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                if (ModelState["BloodGroup"].Errors.Count > 0)
                {
                    ModelState.AddModelError(string.Empty, "Kan grubunu seçiniz!");
                }
                if (ModelState["Gender"].Errors.Count > 0)
                {
                    ModelState.AddModelError(string.Empty, "Cinsiyet seçiniz!");
                }
                TempData["Result"] = "modelinvalid";
                return View(model); // Model valid değil ise validation errorları ile birlikte register sayfasına geri döner
            }

            var multipartContent = new MultipartFormDataContent();

            var properties = typeof(UserRegisterDTO).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(model)?.ToString() ?? string.Empty;
                var stringContent = new StringContent(value, Encoding.UTF8, MediaTypeNames.Text.Plain);
                multipartContent.Add(stringContent, property.Name);
            }

            if (model.UploadPath != null && model.UploadPath.Length > 0) // Eğer profil fotoğrafı yüklenmiş ise multipartContent'e ekle
            {
                string fileExtension = Path.GetExtension(model.UploadPath.FileName).ToLower();

                if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".jpeg")
                {
                    ModelState.AddModelError(string.Empty, "Yüklediğiniz profil fotoğrafının uzantısı '.png', '.jpg' veya '.jpeg' olmalıdır.");
                    return View(model);
                }

                var imageContent = new StreamContent(model.UploadPath.OpenReadStream());
                multipartContent.Add(imageContent, "UploadPath", model.UploadPath.FileName);
            }

            HttpResponseMessage response = await _httpClient.PostAsync("/api/Account/RegisterCompanyManager", multipartContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("RegistrationSuccessful");
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
            returnUrl = returnUrl is null ? "/Employee/Home" : returnUrl;
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
                var apiResponse = await response.Content.ReadAsStringAsync();
                dynamic parsedResponse = JsonConvert.DeserializeObject(apiResponse);
                string userId = parsedResponse.userId;
                string name = parsedResponse.name;
                string surname = parsedResponse.surname;
                string companyId = parsedResponse.companyId;
                string departmentId = parsedResponse.departmentId;
                string managerId = parsedResponse.managerId;
                string imagePath = parsedResponse.imagePath;
                var roles = parsedResponse.roles;

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Email, model.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
                claims.Add(new Claim(ClaimTypes.Name, name));
                claims.Add(new Claim(ClaimTypes.Surname, surname));
                claims.Add(new Claim("CompanyId", companyId));
                claims.Add(new Claim("DepartmentId", departmentId));
                claims.Add(new Claim("ManagerId", managerId));
                claims.Add(new Claim("ImagePath", imagePath));

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return Redirect(model.ReturnUrl ?? "/");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Giriş işlemi başarısız. Lütfen tekrar deneyiniz.");
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

        [AllowAnonymous]
        public IActionResult RegistrationSuccessful()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Lütfen geçerli bir e-mail adresi giriniz!");
                return View(model);
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Account/ForgotPassword/{model.Email}");
            if (response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("SuccessMessage", "Şifre sıfırlama linki e-mail adresinize gönderildi.");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("ErrorMessage", "Böyle bir e-mail kaydı bulunamadı!");
                return View(model);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, string token)
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                // Redirect to the ResetPassword action after successful logout
                return RedirectToAction("ResetPassword", new { id = id, token = token });
            }

            string tokenFromQueryString = Request.Query["token"];
            string idFromQueryString = Request.Query["id"];
            return token == null ? View("Error") : View(new ResetPasswordDTO() { Id = idFromQueryString, Token = tokenFromQueryString });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu tekrar deneyiniz!");
                return View(model);
            }
            var json = System.Text.Json.JsonSerializer.Serialize(model);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/api/Account/ResetPassword", content);
            if (response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("SuccessMessage", "Şifreniz başarılı bir şekilde yenilenmiştir.");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("ErrorMessage", "Bağlantı linkiniz geçersiz olduğu için şifreniz yenilenemedi!");
                return View(model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProfileImage(UpdateProfileImageDTO model)
        {
            model.Id = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            using var multipartContent = new MultipartFormDataContent
                {
                    { new StringContent(model.Id.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "Id" }
                };

            var imageContent = new StreamContent(model.UploadPath.OpenReadStream());
            multipartContent.Add(imageContent, "UploadPath", model.UploadPath.FileName);

            var response = await _httpClient.PostAsync($"api/Account/UpdateProfileImage/", multipartContent);
            if (response.IsSuccessStatusCode)
            {
                var identity = (ClaimsIdentity)User.Identity;
                var existingImageUrlClaim = identity.FindFirst("ImagePath");
                if (existingImageUrlClaim != null)
                {
                    identity.RemoveClaim(existingImageUrlClaim);
                }

                string imageUrl = await response.Content.ReadAsStringAsync();
                identity.AddClaim(new Claim("ImagePath", imageUrl));

                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

                return Json(new { imageUrl });
            }
            return RedirectToAction("Home", "Employee");
        }
    }
}
