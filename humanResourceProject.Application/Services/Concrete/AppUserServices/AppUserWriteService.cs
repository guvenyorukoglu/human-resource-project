using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IDepartmantServices;
using humanResourceProject.Application.Services.Abstract.IImageServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


namespace humanResourceProject.Application.Services.Concrete.AppUserServices
{
    public class AppUserWriteService : BaseWriteService<AppUser>, IAppUserWriteService
    {
        private readonly IBaseWriteRepository<AppUser> _writeRepository;
        private readonly IBaseReadRepository<AppUser> _readRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IBaseReadRepository<Job> _jobReadRepository;
        private readonly IBaseReadRepository<Department> _departmentReadRepository;
        private readonly IAppUserReadService _appUserReadService;
        private readonly IDepartmentReadService _departmentReadService;

        public AppUserWriteService(IBaseWriteRepository<AppUser> writeRepository, IBaseReadRepository<AppUser> readRepository, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IImageService imageService, IConfiguration configuration, IBaseReadRepository<Job> jobReadRepository, IDepartmentReadService departmentReadService, IAppUserReadService appUserReadService) : base(writeRepository, readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
            _userManager = userManager;
            _imageService = imageService;
            _configuration = configuration;
            _jobReadRepository = jobReadRepository;
            _departmentReadService = departmentReadService;
            _appUserReadService = appUserReadService;
        }

        public async Task<UpdateUserDTO> GetUpdateUserDTOById(Guid id)
        {
            AppUser appUser = _readRepository.GetSingleDefault(x => x.Id == id).Result;
            if (appUser == null)
                return null;

            UpdateUserDTO updateUserDTO = _mapper.Map<UpdateUserDTO>(appUser);
            updateUserDTO.Jobs = await _jobReadRepository.GetFilteredList(
                               select: x => new JobVM
                               {
                                   Id = x.Id,
                                   Title = x.Title,
                                   Description = x.Description
                               },
                               where: x => x.Status != Status.Deleted && x.Status != Status.Inactive && x.CompanyId == appUser.CompanyId,
                               orderBy: x => x.OrderBy(x => x.Title));

            updateUserDTO.Departments = _departmentReadService.GetDepartmentsByCompanyId(appUser.CompanyId).Result;

            updateUserDTO.Managers = _appUserReadService.GetManagersByCompanyId(appUser.CompanyId).Result;

            return updateUserDTO;
        }

        public async Task<IdentityResult> RegisterPersonel(CreateEmployeeDTO model)
        {
            if (model == null)
                return IdentityResult.Failed();

            AppUser newUser = _mapper.Map<AppUser>(model);
            newUser.UserName = newUser.Email;
            newUser.CreateDate = DateTime.Now;
            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Personel");
            }
            return result;
        }
        public async Task<IdentityResult> RegisterPersonelManager(CreateEmployeeDTO model)
        {
            if (model == null)
                return IdentityResult.Failed();

            AppUser newUser = _mapper.Map<AppUser>(model);
            newUser.UserName = newUser.Email;
            newUser.CreateDate = DateTime.Now;
            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(newUser, "Manager");

            }
            return result;
        }

        public async Task<IdentityResult> RegisterCompanyManager(UserRegisterDTO model)
        {
            if (model == null)
                return IdentityResult.Failed();

            model = await _imageService.UploadImageToAzure(model);

            AppUser newUser = _mapper.Map<AppUser>(model);
            newUser.UserName = newUser.Email;
            newUser.CreateDate = DateTime.Now;
            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
                await _userManager.AddToRoleAsync(newUser, "CompanyManager");

            return result;
        }
        public async Task<IdentityResult> Update(UpdateUserDTO model)
        {
            AppUser userBeUpdated = await _readRepository.GetSingleDefault(x => x.Id == model.Id);
            if (userBeUpdated == null)
                return IdentityResult.Failed();

            userBeUpdated.FirstName = model.FirstName.Trim() ?? userBeUpdated.FirstName;
            userBeUpdated.MiddleName = model.MiddleName ?? "";
            userBeUpdated.LastName = model.LastName.Trim() ?? userBeUpdated.LastName;
            userBeUpdated.Email = model.Email.Trim().ToLowerInvariant() ?? userBeUpdated.Email;
            userBeUpdated.NormalizedEmail = model.Email.Trim().ToUpperInvariant() ?? userBeUpdated.NormalizedEmail;
            userBeUpdated.UserName = model.Email.Trim().ToLowerInvariant() ?? userBeUpdated.UserName;
            userBeUpdated.NormalizedUserName = model.Email.Trim().ToUpperInvariant() ?? userBeUpdated.NormalizedUserName;
            userBeUpdated.PhoneNumber = model.PhoneNumber.Trim() ?? userBeUpdated.PhoneNumber;
            userBeUpdated.Birthdate = model.Birthdate;
            userBeUpdated.Address = model.Address.Trim() ?? userBeUpdated.Address;
            userBeUpdated.IdentificationNumber = model.IdentificationNumber.Trim() ?? userBeUpdated.IdentificationNumber;
            userBeUpdated.BloodGroup = model.BloodGroup;
            userBeUpdated.Gender = model.Gender;
            userBeUpdated.JobId = model.JobId;
            userBeUpdated.DepartmentId = model.DepartmentId;
            userBeUpdated.ManagerId = model.ManagerId;
            userBeUpdated.UpdateDate = DateTime.Now;
            userBeUpdated.Status = Status.Modified;

            var result = await _writeRepository.Update(userBeUpdated);
            if (result)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();

        }

        public async Task<IdentityResult> UpdateProfileImage(Guid id)
        {
            AppUser user = await _readRepository.GetSingleDefault(x => x.Id == id);
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> FireEmployee(Guid id)
        {
            AppUser user = await _readRepository.GetSingleDefault(x => x.Id == id);
            user.Status = Status.Inactive;
            user.UpdateDate = DateTime.Now;


            return await _userManager.UpdateAsync(user);
        }
    }
}
