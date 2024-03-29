﻿using AutoMapper;
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
using System.Globalization;


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
        private readonly IBaseReadRepository<Job> _jobReadRepository;
        private readonly IAppUserReadService _appUserReadService;
        private readonly IBaseWriteRepository<JobLog> _jobLogWriteRepository;
        private readonly IBaseReadRepository<JobLog> _jobLogReadRepository;
        private readonly IDepartmentReadService _departmentReadService;
        private readonly IMailService _mailService;
        private readonly IBaseReadRepository<Company> _companyReadRepository;
        private readonly IBaseWriteRepository<Company> _companyWriteRepository;
        public AppUserWriteService(IBaseWriteRepository<AppUser> writeRepository, IBaseReadRepository<AppUser> readRepository, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IImageService imageService, IBaseReadRepository<Advance> advanceReadRepository, IBaseReadRepository<Leave> leaveReadRepository, IBaseReadRepository<Expense> expenseReadRepository, IBaseWriteRepository<Advance> advanceWriteRepository, IBaseWriteRepository<Expense> expenseWriteRepository, IBaseWriteRepository<Leave> leaveWriteRepository, IBaseReadRepository<Job> jobReadRepository, IDepartmentReadService departmentReadService, IAppUserReadService appUserReadService, IBaseWriteRepository<JobLog> jobLogWriteRepository, IBaseReadRepository<JobLog> jobLogReadRepository, IMailService mailService, IBaseReadRepository<Company> companyReadRepository, IBaseWriteRepository<Company> companyWriteRepository) : base(writeRepository, readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
            _userManager = userManager;
            _imageService = imageService;
            _advanceReadRepository = advanceReadRepository;
            _leaveReadRepository = leaveReadRepository;
            _expenseReadRepository = expenseReadRepository;
            _advanceWriteRepository = advanceWriteRepository;
            _expenseWriteRepository = expenseWriteRepository;
            _leaveWriteRepository = leaveWriteRepository;
            _jobReadRepository = jobReadRepository;
            _departmentReadService = departmentReadService;
            _appUserReadService = appUserReadService;
            _jobLogWriteRepository = jobLogWriteRepository;
            _jobLogReadRepository = jobLogReadRepository;
            _mailService = mailService;
            _companyReadRepository = companyReadRepository;
            _companyWriteRepository = companyWriteRepository;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
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

            updateUserDTO.Managers = _appUserReadService.GetManagersByCompanyId(appUser.CompanyId).Result.Where(x => x.Id != appUser.Id).ToList();

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
            AppUser user = await _readRepository.GetDefault(x => x.Email == model.Email);
            if (model == null || user != null)
                return IdentityResult.Failed();

            AppUser newUser = _mapper.Map<AppUser>(model);
            newUser.UserName = newUser.Email;
            newUser.CreateDate = DateTime.Now;
            newUser.ManagerId = model.ManagerId == Guid.Empty ? null : model.ManagerId;
            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Personel");
                JobLog jobLog = new JobLog
                {
                    EmployeeId = newUser.Id,
                    JobId = newUser.JobId,      //TODO: JobId null olmamalı (hem userda hem de JobLogda)
                    DateOfStart = model.StartDateOfEmployment, //TODO: işe başlama tarihi input olarak alınmalı
                    DateOfTermination = null,
                    Status = Status.Active,
                    CreateDate = DateTime.Now
                };
                var response = await _jobLogWriteRepository.Insert(jobLog);
                if (!response)
                    return IdentityResult.Failed();
            }
            return result;
        }
        public async Task<IdentityResult> RegisterPersonelManager(CreateEmployeeDTO model)
        {
            AppUser user = await _readRepository.GetDefault(x => x.Email == model.Email);
            if (model == null || user != null)
                return IdentityResult.Failed();

            AppUser newUser = _mapper.Map<AppUser>(model);
            newUser.UserName = newUser.Email;
            newUser.CreateDate = DateTime.Now;
            newUser.ManagerId = model.ManagerId == Guid.Empty ? null : model.ManagerId;
            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Manager");
                JobLog jobLog = new JobLog
                {
                    EmployeeId = newUser.Id,
                    JobId = newUser.JobId,      //TODO: JobId null olmamalı (hem userda hem de JobLogda)
                    DateOfStart = model.StartDateOfEmployment, //TODO: işe başlama tarihi input olarak alınmalı
                    DateOfTermination = null,
                    Status = Status.Active,
                    CreateDate = DateTime.Now
                };
                var response = await _jobLogWriteRepository.Insert(jobLog);
                if (!response)
                    return IdentityResult.Failed();

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
            {
                await _userManager.AddToRoleAsync(newUser, "CompanyManager");
                Company company = await _companyReadRepository.GetSingleDefault(x => x.Id == model.CompanyId);
                company.ContactFullName = newUser.FullName;
                company.ContactEmail = newUser.Email;
                company.ContactPhoneNumber = newUser.PhoneNumber;

                await _companyWriteRepository.Update(company);
            }

            return result;
        }
        public async Task<IdentityResult> Update(UpdateUserDTO model)
        {
            AppUser userBeUpdated = await _readRepository.GetSingleDefault(x => x.Id == model.Id);
            if (userBeUpdated == null)
                return IdentityResult.Failed();


            if (model.JobId != Guid.Empty && userBeUpdated.JobId == Guid.Empty)
            {
                // Kullanıcıya iş atandıysa JobLog tablosuna kayıt atılır (ilk kaydı yapılırken null atandıysa)
                JobLog jobLog = await _jobLogReadRepository.GetSingleDefault(x => x.EmployeeId == userBeUpdated.Id && x.JobId == Guid.Empty && x.DateOfTermination == null);

                if (jobLog != null)
                {
                    jobLog.JobId = model.JobId;
                    jobLog.DateOfStart = DateTime.Now;
                    jobLog.Status = Status.Active;
                    jobLog.UpdateDate = DateTime.Now;
                    await _jobLogWriteRepository.Update(jobLog);
                }

            }

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
            userBeUpdated.ManagerId = model.ManagerId == Guid.Empty ? null : model.ManagerId;
            userBeUpdated.UpdateDate = DateTime.Now;
            userBeUpdated.Status = Status.Modified;

            var result = await _writeRepository.Update(userBeUpdated);
            if (result)
            {
                return IdentityResult.Success;
            }
            else
                return IdentityResult.Failed();

        }

        public async Task<IdentityResult> UpdateProfileImage(Guid id)
        {
            AppUser user = await _readRepository.GetSingleDefault(x => x.Id == id);
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteEmployee(Guid id)
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

            user.Status = Status.Deleted;
            user.DeleteDate = DateTime.Now;
            var result = await _writeRepository.Delete(user.Id);

            if (result)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> FireEmployee(FireEmployeeDTO model)
        {
            AppUser user = await _readRepository.GetSingleDefault(x => x.Id == model.EmployeeId);
            List<Advance>? advanceList = await _advanceReadRepository.GetDefaults(x => x.EmployeeId == model.EmployeeId);
            List<Expense>? expenseList = await _expenseReadRepository.GetDefaults(x => x.EmployeeId == model.EmployeeId);
            List<Leave>? leaveList = await _leaveReadRepository.GetDefaults(x => x.EmployeeId == model.EmployeeId);

            foreach (var item in advanceList)
            {
                item.Status = Status.Inactive;
                item.AdvanceStatus = RequestStatus.Deleted;
                await _advanceWriteRepository.Update(item);
            }
            foreach (var item in expenseList)
            {
                item.Status = Status.Inactive;
                item.ExpenseStatus = RequestStatus.Deleted;
                await _expenseWriteRepository.Update(item);
            }

            foreach (var item in leaveList)
            {
                item.Status = Status.Inactive;
                item.LeaveStatus = RequestStatus.Deleted;
                await _leaveWriteRepository.Update(item);
            }

            JobLog jobLog = await _jobLogReadRepository.GetSingleDefault(x => x.EmployeeId == model.EmployeeId && x.DateOfTermination == null);
            jobLog.DateOfTermination = model.TerminationDate;
            jobLog.Status = Status.Inactive;
            jobLog.UpdateDate = DateTime.Now;
            jobLog.ReasonForTermination = model.ReasonForTermination;
            await _jobLogWriteRepository.Update(jobLog);

            user.Status = Status.Inactive;
            user.UpdateDate = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                string recipientEmail = user.Email;
                string mailToName = $"{user.FullName}";
                string subject = "İşten Çıkarılma Hk.";
                string body = $"<p>Sayın {user.FullName},</p><p>{((DateTime)jobLog.DateOfTermination).ToShortDateString()} tarihinde iş akdiniz sonlandırılmıştır. İşten çıkarılma sebebiniz;</p><p>{jobLog.ReasonForTermination}</p><p>olarak belirtilmiştir.</p><br><hr><br><h3>Team Monitorease</h3>";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);
                return IdentityResult.Success;
            }
            else
                return IdentityResult.Failed(); 
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
