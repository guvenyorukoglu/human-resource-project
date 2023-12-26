using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.Abstract.IDepartmantServices;
using humanResourceProject.Application.Services.Abstract.IImageServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Presentation.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace humanResourceProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAppUserReadService _appUserReadService;
        private readonly IAppUserWriteService _appUserWriteService;
        private readonly ICompanyWriteService _companyWriteService;
        private readonly ICompanyReadService _companyReadService;
        private readonly IDepartmentReadService _departmentReadService;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBaseWriteService<AppUser> _baseAppUserWriteService;
        private readonly IImageService _imageService;

        public AccountController(IAppUserReadService appUserReadService, IConfiguration configuration, IAppUserWriteService appUserWriteService, ICompanyWriteService companyWriteService, ICompanyReadService companyReadService, IMailService mailService, UserManager<AppUser> userManager, IBaseWriteService<AppUser> baseAppUserWriteService, IDepartmentReadService departmentReadService, IImageService imageService)
        {
            _appUserReadService = appUserReadService;
            _appUserWriteService = appUserWriteService;
            _configuration = configuration;
            _companyWriteService = companyWriteService;
            _companyReadService = companyReadService;
            _mailService = mailService;
            _userManager = userManager;
            _baseAppUserWriteService = baseAppUserWriteService;
            _departmentReadService = departmentReadService;
            _imageService = imageService;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var result = await _appUserReadService.Login(model);
            if (result.Succeeded)
            {
                AppUser appUser = await _appUserReadService.GetSingleDefault(x => x.Email == model.Email);

                if (appUser == null)
                    return Unauthorized();

                var authClaims = new List<Claim> {
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userId = appUser.Id,
                    name = appUser.FirstName,
                    surname = appUser.LastName,
                    departmentId = appUser.DepartmentId,
                    companyId = appUser.CompanyId,
                    managerId = appUser.ManagerId ?? Guid.Empty,
                    imagePath = appUser.ImagePath,
                    roles = await _userManager.GetRolesAsync(appUser)
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("RegisterCompany")]
        public async Task<IActionResult> RegisterCompany([FromBody] CompanyRegisterDTO model)
        {
            var result = await _companyWriteService.RegisterCompany(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var company = await _companyReadService.GetSingleDefault(x => x.CompanyName == model.CompanyName);

            return Ok(company.Id);
        }

        [HttpPost]
        [Route("RegisterCompanyManager")]
        public async Task<IActionResult> RegisterCompanyManager([FromForm] UserRegisterDTO model)
        {
            var result = await _appUserWriteService.RegisterCompanyManager(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            AppUser user = await _userManager.FindByEmailAsync(model.Email);

            string action = Url.Action("SetStatusActive", "Account", new { id = user.Id }, Request.Scheme);
            string mailToName = "Admin";
            string subject = "Yeni Kullanıcı Kayıt Oldu!";
            string body = $"<p>Merhaba Admin,</p><p>Yeni bir kullanıcı uygulamaya kayıt olmuştur.</p><p>Kullanıcının statüsünü aktif yapmak için <a href ='{action}'>buraya</a> tıklayınız.</p><br><hr><br><h3>Team Monitorease</h3>";
            string recipientEmail = "yorukoglu.guven@gmail.com";
            await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);
            //await _mailService.SendUserRegisteredEmail(user, action);

            return Ok("Yeni şirket yöneticisi oluşturuldu.");
        }

        [HttpGet]
        [Route("Logout")]
        public async Task Logout()
        {
            await _appUserReadService.Logout();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:secretKey"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JwtSettings:validIssuer"],
                _configuration["JwtSettings:validAudience"],
                authClaims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: signIn);

            return token;
        }

        [HttpGet]
        [Route("SetStatusActive")]
        public async Task<IActionResult> SetStatusActive(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.Status = Domain.Enum.Status.Active;

            bool userUpdateResult = await _baseAppUserWriteService.Update(user);
            if (userUpdateResult)
            {
                //Send Confirmation Email
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string action = Url.Action("ConfirmEmail", "Account", new { id = user.Id, token }, Request.Scheme);
                string recipientEmail = user.Email;
                string mailToName = $"{user.FirstName} {user.LastName}";
                string subject = "Monitorease Hesabınızı Doğrulayınız!";
                string body = $"<p>Merhaba</p><p>Monitorease hesabınız başarılı bir şekilde oluşturulmuştur.</p><p>Hesabınızı doğrulamak için <a href ='{action}'>buraya</a> tıklayınız.</p><p>Bize her zaman monitorease@gmail.com adresinden ulaşabilirsiniz.</p><br><hr><br><h3>Team Monitorease</h3>";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);
                //await _mailService.SendAccountConfirmEmail(user, action);

                var loginURL = _configuration["HomePage"] + "/account/login";

                return Redirect(loginURL);
            }
            else
                return BadRequest("Hesap aktif hale getirilemedi!");
        }

        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(Guid id, string token)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                var loginUrl = _configuration["HomePage"] + "/account/login";

                return Redirect(loginUrl);
            }
            else
            {
                return BadRequest("Email adresiniz onaylanamadı.");
            }
        }

        [HttpPost]
        [Route("UpdateProfileImage")]
        public async Task<IActionResult> UpdateProfileImage([FromForm] UpdateProfileImageDTO model)
        {
            var result = await _imageService.UpdateProfileImage(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            AppUser user = await _userManager.FindByIdAsync(model.Id.ToString());
            return Ok(user.ImagePath);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Kullanıcı bulunamadı.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            var action = _configuration["HomePage"] + "/Account/ResetPassword?id=" + user.Id + "&token=" + validToken + "";

            string recipientEmail = user.Email;
            string mailToName = $"{user.FirstName} {user.LastName}";
            string subject = "Monitorease Şifre Sıfırlama";
            string body = $"<p>Merhaba {user.FirstName} {user.LastName},</p><p>Şifrenizi sıfırlamak için <a href ='{action}'>buraya</a> tıklayabilirsiniz. Linkin geçerlilik süresi 24 saattir.</p><p>Bize her zaman monitorease@gmail.com adresinden ulaşabilirsiniz.</p><br><hr><br><h3>Team Monitorease</h3>";
            await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);
            //await _mailService.SendForgotPasswordEmail(user, resetLink);
            return Ok($"Şifre sıfırlama linki {user.Email} adresine gönderildi! Linke tıklayıp şifrenizi sıfırlayabilirsiniz.");
        }

        [HttpGet]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest("Kullanıcı bulunamadı.");

            var model = new ResetPasswordDTO
            {
                Id = user.Id.ToString(),
                Token = token
            };

            return Ok(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return BadRequest("Kullanıcı bulunamadı.");

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            var normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            user.Status = Domain.Enum.Status.Active;
            user.EmailConfirmed = true;
            await _appUserWriteService.Update(user);
            return Ok("Şifreniz başarıyla değiştirildi.");


        }
    }
}
