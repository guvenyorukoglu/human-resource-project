﻿using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IImageServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
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
  
        public AppUserWriteService(IBaseWriteRepository<AppUser> writeRepository, IBaseReadRepository<AppUser> readRepository, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IImageService imageService, IConfiguration configuration) : base(writeRepository, readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
            _userManager = userManager;
            _imageService = imageService;
            _configuration = configuration;
          
        }

        public async Task<UpdateUserDTO> GetUpdateUserDTOById(Guid id)
        {
            AppUser appUser = _readRepository.GetSingleDefault(x => x.Id == id).Result;
            if (appUser == null)
                return null;

            UpdateUserDTO updateUserDTO = _mapper.Map<UpdateUserDTO>(appUser);
            return updateUserDTO;
        }
         public async Task<UpdateProfileDTO> GetUpdateProfileDTOById(Guid id)
        {
            AppUser appUser = _readRepository.GetSingleDefault(x => x.Id == id).Result;
            if (appUser == null)
                return null;

            UpdateProfileDTO updateProfileDTO = _mapper.Map<UpdateProfileDTO>(appUser);
            return updateProfileDTO;
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
            user.Status = Status.Inactive;
            user.UpdateDate = DateTime.Now;


            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateProfile(UpdateProfileDTO model)
        {
            AppUser updatedUser = await _readRepository.GetSingleDefault(x => x.Id == model.Id);
            if (updatedUser == null)
                return IdentityResult.Failed();

            else
            {
                _writeRepository.DetachEntity(updatedUser);
                updatedUser.BloodGroup = model.BloodGroup;
                updatedUser.Address = model.Address;
                updatedUser.PhoneNumber = model.PhoneNumber;


                updatedUser.Status = Domain.Enum.Status.Modified;

                updatedUser.UpdateDate = DateTime.Now;


                var result = await _writeRepository.Update(updatedUser);
                if (result)
                    return IdentityResult.Success;
                else
                    return IdentityResult.Failed();
            }
        }
    }
}
