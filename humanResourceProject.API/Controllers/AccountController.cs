using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.Concrete.CompanyServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        public AccountController(IAppUserReadService appUserReadService, IConfiguration configuration, IAppUserWriteService appUserWriteService, ICompanyWriteService companyWriteService, ICompanyReadService companyReadService)
        {
            _appUserReadService = appUserReadService;
            _appUserWriteService = appUserWriteService;
            _configuration = configuration;
            _companyWriteService = companyWriteService;
            _companyReadService = companyReadService;
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
                    companyId = appUser.CompanyId,
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

        //[HttpPost]
        //[Route("UpdateProfileImage")]
        //public async Task<IActionResult> UpdateProfileImage([FromBody] Guid id) 
        //{
        //    return Ok(await _appUserWriteService.UpdateProfileImage(id));
        //}

    }
}
