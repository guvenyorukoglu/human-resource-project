using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBaseWriteService<AppUser> _baseAppUserWriteService;

        public AccountController(IAppUserReadService appUserReadService, IConfiguration configuration, IAppUserWriteService appUserWriteService, ICompanyWriteService companyWriteService, ICompanyReadService companyReadService, IMailService mailService, UserManager<AppUser> userManager, IBaseWriteService<AppUser> baseAppUserWriteService)
        {
            _appUserReadService = appUserReadService;
            _appUserWriteService = appUserWriteService;
            _configuration = configuration;
            _companyWriteService = companyWriteService;
            _companyReadService = companyReadService;
            _mailService = mailService;
            _userManager = userManager;
            _baseAppUserWriteService = baseAppUserWriteService;
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
                    imagePath = appUser.ImagePath
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
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromForm] UserRegisterDTO model)
        {
            var result = await _appUserWriteService.RegisterCompanyManager(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            AppUser user = await _userManager.FindByEmailAsync(model.Email);

            string action = Url.Action("SetStatusActive", "Account", new { id = user.Id }, Request.Scheme);
            await _mailService.SendUserRegisteredEmail(user, action);

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
                await _mailService.SendAccountConfirmEmail(user, action);

                var loginURL = "https://localhost:7180/account/login"; //Local
                //var loginURL = "https://monitorease.azurewebsites.net/account/login"; //Azure

                return Redirect(loginURL);
            }
            else
                return BadRequest("Hesap aktif hale getirilemedi!");
        }

        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(Guid id, string token)
        {
            //var id = data["id"].ToObject<Guid>();
            //var token = data["token"].ToString();
            var user = await _userManager.FindByIdAsync(id.ToString());
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                var loginUrl = "https://localhost:7180/account/login"; //Local
                //var loginUrl = "https://monitorease.azurewebsites.net/account/login"; //Azure
                return Redirect(loginUrl);
            }
            else
            {
                return BadRequest("Email adresiniz onaylanamadı.");
            }
        }

        //[HttpPost]
        //[Route("UpdateProfileImage")]
        //public async Task<IActionResult> UpdateProfileImage([FromBody] Guid id) 
        //{
        //    return Ok(await _appUserWriteService.UpdateProfileImage(id));
        //}

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Kullanıcı bulunamadı.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action(nameof(ResetPassword), "Account", new { id = user.Id, token }, Request.Scheme);
            await _mailService.SendForgotPasswordEmail(user, resetLink);
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

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok("Şifreniz başarıyla değiştirildi.");


        }
    }
}
