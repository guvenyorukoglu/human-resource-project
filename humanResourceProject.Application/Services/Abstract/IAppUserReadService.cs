using humanResourceProject.Application.Services.Abstract;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.DTO.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Application.Services.AppUserServices
{
    public interface IAppUserReadService : IBaseReadService<AppUser>
    {

        Task<SignInResult> Login(LoginDTO model);

        Task Logout();
    }
}
