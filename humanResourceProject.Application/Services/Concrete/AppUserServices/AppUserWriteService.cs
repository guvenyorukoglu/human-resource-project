using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IImageServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;


namespace humanResourceProject.Application.Services.Concrete.AppUserServices
{
    public class AppUserWriteService : BaseWriteService<AppUser>, IAppUserWriteService
    {
        private readonly IBaseWriteRepository<AppUser> _writeRepository;
        private readonly IBaseReadRepository<AppUser> _readRepository;
        private readonly IBaseReadRepository<Advance> _advanceReadRepository;
        private readonly IBaseReadRepository<Expense> _expenseReadRepository;
        private readonly IBaseReadRepository<Leave> _leaveReadRepository;
        private readonly IBaseWriteRepository<Advance> _advanceWriteRepository;
        private readonly IBaseWriteRepository<Expense> _expenseWriteRepository;
        private readonly IBaseWriteRepository<Leave> _leaveWriteRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public AppUserWriteService(IBaseWriteRepository<AppUser> writeRepository, IBaseReadRepository<AppUser> readRepository, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IImageService imageService, IConfiguration configuration, IBaseReadRepository<Advance> advanceReadRepository, IBaseReadRepository<Leave> leaveReadRepository, IBaseReadRepository<Expense> expenseReadRepository, IBaseWriteRepository<Advance> advanceWriteRepository, IBaseWriteRepository<Expense> expenseWriteRepository, IBaseWriteRepository<Leave> leaveWriteRepository) : base(writeRepository, readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
            _userManager = userManager;
            _imageService = imageService;
            _configuration = configuration;
            _advanceReadRepository = advanceReadRepository;
            _leaveReadRepository = leaveReadRepository;
            _expenseReadRepository = expenseReadRepository;
            _advanceWriteRepository = advanceWriteRepository;
            _expenseWriteRepository = expenseWriteRepository;
            _leaveWriteRepository = leaveWriteRepository;
        }

        public async Task<UpdateUserDTO> GetUpdateUserDTOById(Guid id)
        {
            AppUser appUser = _readRepository.GetSingleDefault(x => x.Id == id).Result;
            if (appUser == null)
                return null;

            UpdateUserDTO updateUserDTO = _mapper.Map<UpdateUserDTO>(appUser);
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
            AppUser updatedUser = await _readRepository.GetSingleDefault(x => x.Id == model.Id);
            if (updatedUser == null)
                return IdentityResult.Failed();
            AppUser newUser = _mapper.Map<AppUser>(model);
            updatedUser.Email = model.Email.Trim().ToLowerInvariant() ?? updatedUser.Email;
            updatedUser.NormalizedEmail = model.Email.Trim().ToUpperInvariant() ?? updatedUser.NormalizedEmail;
            updatedUser.UserName = model.Email.Trim().ToLowerInvariant() ?? updatedUser.UserName;
            updatedUser.NormalizedUserName = model.Email.Trim().ToUpperInvariant() ?? updatedUser.NormalizedUserName;
            updatedUser.UpdateDate = DateTime.Now;
            updatedUser.Status = Status.Modified;

            var result = await _writeRepository.Update(updatedUser);
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
            List<Advance>? advanceList = await _advanceReadRepository.GetDefaults(x => x.EmployeeId == id);
            List<Expense>? expenseList = await _expenseReadRepository.GetDefaults(x => x.EmployeeId == id);
            List<Leave>? leaveList = await _leaveReadRepository.GetDefaults(x => x.EmployeeId == id);                        

            foreach (var item in advanceList)
            {
                item.Status = Status.Deleted;
                item.AdvanceStatus = RequestStatus.Deleted;
                await _advanceWriteRepository.Update(item);
            }


            foreach (var item in expenseList)
            {
                item.Status = Status.Deleted;
                item.ExpenseStatus = RequestStatus.Deleted;
                await _expenseWriteRepository.Update(item);
            }

            foreach (var item in leaveList)
            {
                item.Status = Status.Deleted;
                item.LeaveStatus = RequestStatus.Deleted;
                await _leaveWriteRepository.Update(item);
            }

            user.Status = Status.Inactive;
            user.UpdateDate = DateTime.Now;

            return await _userManager.UpdateAsync(user);
        }
    }
}
