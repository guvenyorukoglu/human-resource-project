using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Abstract.IAppUserServices
{
    public interface IAppUserWriteService : IBaseWriteService<AppUser>
    {
        Task<IdentityResult> Register(UserRegisterDTO model);
        Task<bool> Create(UserRegisterDTO model);
        Task Update(UpdateUserDTO model);

    }
}
