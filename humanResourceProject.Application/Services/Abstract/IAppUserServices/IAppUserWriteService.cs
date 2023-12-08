using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository;
using humanResourceProject.DTO.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Application.Services.Abstract.IAppUserServices
{
    public interface IAppUserWriteService : IBaseWriteService<AppUser>
    {
        Task<IdentityResult> Register(UserRegisterDTO model);

    }
}
