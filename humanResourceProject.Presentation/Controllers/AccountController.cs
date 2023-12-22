using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            HttpResponseMessage response = await _httpClient.GetAsync($"api/Job/GetAllJobs/");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                dynamic jobList = JsonConvert.DeserializeObject(apiResponse);

                foreach (var job in jobList)
                {
                    jobs.Add(new JobVM()
                    {
                        Id = job.id,
                        Title = job.title,
                        Description = job.description
                    });
                }
                userRegisterDTO.Jobs = jobs;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu. Tekrar deneyiniz!");
                return View(userRegisterDTO);
            }
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
                TempData["Result"] = "modelinvalid";
                model.Jobs = jobs;
                return View(model); // Model valid değil ise validation errorları ile birlikte register sayfasına geri döner
            }

            Guid departmentId = await CreateDepartment(model);
            CompanyManagerRegisterDTO companyManagerRegisterDTO = new CompanyManagerRegisterDTO()
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName ?? "",
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                PhoneNumber = model.PhoneNumber,
                Birthdate = model.Birthdate,
                Address = model.Address,
                IdentificationNumber = model.IdentificationNumber,
                BloodGroup = model.BloodGroup,
                Gender = model.Gender,
                JobId = model.JobId,
                ImagePath = model.ImagePath ?? "",
                UploadPath = model.UploadPath,
                DepartmentId = departmentId
            };

            var multipartContent = new MultipartFormDataContent();

            var properties = typeof(CompanyManagerRegisterDTO).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(companyManagerRegisterDTO)?.ToString() ?? string.Empty;
                var stringContent = new StringContent(value, Encoding.UTF8, MediaTypeNames.Text.Plain);
                multipartContent.Add(stringContent, property.Name);
            }


            //using var multipartContent = new MultipartFormDataContent
            //    {
            //        { new StringContent(companyManagerRegisterDTO.FirstName, Encoding.UTF8, MediaTypeNames.Text.Plain), "FirstName" },
            //        { new StringContent(companyManagerRegisterDTO.MiddleName, Encoding.UTF8, MediaTypeNames.Text.Plain), "MiddleName" },
            //        { new StringContent(companyManagerRegisterDTO.LastName, Encoding.UTF8, MediaTypeNames.Text.Plain), "LastName" },
            //        { new StringContent(companyManagerRegisterDTO.Email, Encoding.UTF8, MediaTypeNames.Text.Plain), "Email" },
            //        { new StringContent(companyManagerRegisterDTO.Password, Encoding.UTF8, MediaTypeNames.Text.Plain), "Password" },
            //        { new StringContent(companyManagerRegisterDTO.ConfirmPassword, Encoding.UTF8, MediaTypeNames.Text.Plain), "ConfirmPassword" },
            //        { new StringContent(companyManagerRegisterDTO.PhoneNumber, Encoding.UTF8, MediaTypeNames.Text.Plain), "PhoneNumber" },
            //        { new StringContent(companyManagerRegisterDTO.Birthdate.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "Birthdate" },
            //        { new StringContent(companyManagerRegisterDTO.Address, Encoding.UTF8, MediaTypeNames.Text.Plain), "Address" },
            //        { new StringContent(companyManagerRegisterDTO.IdentificationNumber, Encoding.UTF8, MediaTypeNames.Text.Plain), "IdentificationNumber" },
            //        { new StringContent(companyManagerRegisterDTO.BloodGroup.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "BloodGroup" },
            //        { new StringContent(companyManagerRegisterDTO.Gender.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "Gender" },
            //        { new StringContent(companyManagerRegisterDTO.JobId.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "JobId" },
            //        { new StringContent(companyManagerRegisterDTO.ImagePath, Encoding.UTF8, MediaTypeNames.Text.Plain), "ImagePath" },
            //        { new StringContent(companyManagerRegisterDTO.DepartmentId.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "DepartmentId" }
            //    };


            if (companyManagerRegisterDTO.UploadPath != null && companyManagerRegisterDTO.UploadPath.Length > 0) // Eğer profil fotoğrafı yüklenmiş ise multipartContent'e ekle
            {
                string fileExtension = Path.GetExtension(companyManagerRegisterDTO.UploadPath.FileName).ToLower();

                if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".jpeg")
                {
                    ModelState.AddModelError(string.Empty, "Yüklediğiniz profil fotoğrafının uzantısı '.png', '.jpg' veya '.jpeg' olmalıdır.");
                    model.Jobs = jobs;
                    return View(model);
                }

                var imageContent = new StreamContent(companyManagerRegisterDTO.UploadPath.OpenReadStream());
                multipartContent.Add(imageContent, "UploadPath", companyManagerRegisterDTO.UploadPath.FileName);
            }

            HttpResponseMessage response = await _httpClient.PostAsync("/api/Account/RegisterCompanyManager", multipartContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("RegistrationSuccessful");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu. Tekrar deneyiniz!");
                model.Jobs = jobs;
                return View(model);
            }

        }

        private async Task<Guid> CreateDepartment(UserRegisterDTO model) // Register işlemi sırasında departman oluşturur ve departman id'sini döner
        {
            DepartmentDTO departmentDTO = new DepartmentDTO()
            {
                DepartmentName = model.DepartmentName,
                Description = model.DepartmentDescription ?? "",
                CompanyId = model.CompanyId
            };

            var json = JsonConvert.SerializeObject(departmentDTO);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/api/Department/CreateDepartment", content);
            if (!response.IsSuccessStatusCode)
                return Guid.Empty;

            var departmentId = await response.Content.ReadAsStringAsync();
            departmentId = departmentId.Replace("\"", "");
            return Guid.Parse(departmentId);
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
                string imagePath = parsedResponse.imagePath;
                var roles = parsedResponse.roles;

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Email, model.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
                claims.Add(new Claim(ClaimTypes.Name, name));
                claims.Add(new Claim(ClaimTypes.Surname, surname));
                claims.Add(new Claim("CompanyId", companyId));
                claims.Add(new Claim("DepartmentId", departmentId));
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

        [AllowAnonymous]
        public IActionResult RegistrationSuccessful()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult ResetPassword(string id, string token)
        {
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
                return View(model);
            }
            var json = System.Text.Json.JsonSerializer.Serialize(model);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/api/Account/ResetPassword", content);
            if (response.IsSuccessStatusCode)
            {
                return View("ResetPasswordConfirmation");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bir Hata Oluştu Tekrar Deneyiniz!");
                return View(model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProfileImage(UpdateProfileImageDTO model)
        {
            model.Id =  Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            using var multipartContent = new MultipartFormDataContent
                {
                    { new StringContent(model.Id.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "Id" }
                };

            var imageContent = new StreamContent(model.UploadPath.OpenReadStream());
            multipartContent.Add(imageContent, "UploadPath", model.UploadPath.FileName);

            var response = await _httpClient.PostAsync($"api/Account/UpdateProfileImage/", multipartContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Home", "Employee");
            }
            return View("Error");
        }

    }
}
