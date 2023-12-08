using humanResourceProject.Application.Services.Abstract.IAppUserServices;
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
        private readonly IConfiguration _configuration;

        public AccountController(IAppUserReadService appUserReadService, IConfiguration configuration, IAppUserWriteService appUserWriteService)
        {
            _appUserReadService = appUserReadService;
            _appUserWriteService = appUserWriteService;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var result = await _appUserReadService.Login(model);
            if (result.Succeeded)
            {
                var authClaims = new List<Claim> {
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO model)
        {
            return Ok(await _appUserWriteService.Register(model));
        }

        [HttpPost]
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



    }
}
