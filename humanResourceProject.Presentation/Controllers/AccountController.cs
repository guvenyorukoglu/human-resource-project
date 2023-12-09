using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace humanResourceProject.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IBaseWriteService<AppUser> _appUserWriteService;
        private readonly IBaseReadService<AppUser> _appUserReadService;
        private readonly IAppUserWriteService _userWriteService;
        private readonly IAppUserReadService _userReadService;

        public AccountController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher, IBaseWriteService<AppUser> appUserWriteService, IAppUserWriteService userWriteService, IAppUserReadService userReadService, IBaseReadService<AppUser> appUserReadService)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _appUserWriteService = appUserWriteService;
            _userWriteService = userWriteService;
            _userReadService = userReadService;
            _appUserReadService = appUserReadService;
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
            if (ModelState.IsValid)
            {
                IdentityResult identityResult = await _userWriteService.Register(model);

                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (IdentityError error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
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
            if (ModelState.IsValid)
            {
                var result = await _userReadService.Login(model);
                if (result.Succeeded)
                {

                    AppUser appUser = await _appUserReadService.GetSingleDefault(x => x.Email == model.Email);

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Email, appUser.Email));

                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return Redirect(model.ReturnUrl ?? "/");
                }
                else
                    ModelState.AddModelError("", "Email or password is wrong!");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userReadService.Logout();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
