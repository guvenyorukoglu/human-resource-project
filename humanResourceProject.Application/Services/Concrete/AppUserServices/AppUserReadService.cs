using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                    Gender = x.Gender,
                    Title = x.Company.Jobs.FirstOrDefault(j => j.Id == x.JobId).Title
                },
                where: x => x.CompanyId == companyId && (x.Status != Domain.Enum.Status.Inactive && x.Status != Domain.Enum.Status.Deleted),
                orderBy: x => x.OrderBy(x => x.CreateDate),
                include: x => x.Include(x => x.Company).ThenInclude(x => x.Jobs));
        }

        public async Task<List<PersonelVM>> GetEmployeesByManagerId(Guid managerId)
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
                    Gender = x.Gender,
                    Title = x.Company.Jobs.FirstOrDefault(j => j.Id == x.JobId).Title
                },
                where: x => x.ManagerId == managerId && (x.Status != Domain.Enum.Status.Inactive && x.Status != Domain.Enum.Status.Deleted),
                orderBy: x => x.OrderBy(x => x.CreateDate),
                include: x => x.Include(x => x.Company).ThenInclude(x => x.Jobs));
        }

        public async Task<List<ManagerVM>> GetManagersByCompanyId(Guid companyId)
        {
            List<AppUser> managers = (List<AppUser>)await _userManager.GetUsersInRoleAsync("Manager");
            List<AppUser> companyManager = (List<AppUser>)await _userManager.GetUsersInRoleAsync("CompanyManager");

            managers.AddRange(companyManager);

            var filteredManagers = managers.Where(x => x.CompanyId == companyId && (x.Status != Domain.Enum.Status.Inactive && x.Status != Domain.Enum.Status.Deleted))
                .Select(x => new ManagerVM
                {
                    Id = x.Id,
                    FullName = x.FirstName + " " + x.LastName
                })
                .OrderBy(x => x.FullName)
                .ToList();

            return filteredManagers;
        }

        public async Task<List<ManagerVM>> GetManagersByDepartmentId(Guid deparmentId)
        {
            return await _readRepository.GetFilteredList(
                select: x => new ManagerVM()
                {
                    Id = x.Id,
                    FullName = x.FirstName + " " + x.LastName
                },
                where: x => x.DepartmentId == deparmentId && (x.Status != Domain.Enum.Status.Inactive && x.Status != Domain.Enum.Status.Deleted));
        }

        public async Task<SignInResult> Login(LoginDTO model)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(model.Email);
            if (appUser == null || appUser.Status == Domain.Enum.Status.Inactive || appUser.Status == Domain.Enum.Status.Deleted)
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
