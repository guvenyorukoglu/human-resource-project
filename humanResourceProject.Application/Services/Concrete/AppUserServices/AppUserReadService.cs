using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Concrete.AppUserServices
{
    public class AppUserReadService : BaseReadService<AppUser>, IAppUserReadService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBaseReadRepository<AppUser> _readRepository;

        public AppUserReadService(IBaseReadRepository<AppUser> readRepository, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) : base(readRepository)
        {
            _readRepository = readRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<List<PersonelVM>> GetEmployeesByCompanyId(Guid companyId)
        {
            return await _readRepository.GetFilteredList(
                select: x => new PersonelVM()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    MiddleName = x.MiddleName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Title = x.Title,
                    Job = x.Job
                },
                where: x => x.CompanyId == companyId && x.Status != Domain.Enum.Status.Inactive,
                orderBy: x => x.OrderBy(x => x.FirstName));
        }

        public async Task<SignInResult> Login(LoginDTO model)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(model.Email);
            if (appUser == null)
                return SignInResult.Failed;

            var result = await _signInManager.PasswordSignInAsync(appUser, model.Password, false, false);
            return result;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
