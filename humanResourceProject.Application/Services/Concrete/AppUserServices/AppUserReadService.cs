using humanResourceProject.Application.Services.Abstract;
using humanResourceProject.Application.Services.AppUserServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository;
using humanResourceProject.DTO.DTOs;
using humanResourceProject.Infrastructure.Repositories.AppUserRepos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Application.Services.Concrete.AppUserServices
{
    public class AppUserReadService : BaseReadService<AppUser>, IAppUserReadService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IBaseReadRepository<AppUser> _readRepository;

        public AppUserReadService(IBaseReadRepository<AppUser> readRepository, SignInManager<AppUser> signInManager) : base(readRepository)
        {
            _readRepository = readRepository;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> Login(LoginDTO model)
        {
            AppUser appUser = await _readRepository.GetSingleDefault(x => x.Email == model.Email);
            if (appUser == null)
                return SignInResult.Failed;
            return await _signInManager.PasswordSignInAsync(appUser, model.Password, false, false);


        }

        public Task Logout()
        {
            return _signInManager.SignOutAsync();
        }
    }
}
