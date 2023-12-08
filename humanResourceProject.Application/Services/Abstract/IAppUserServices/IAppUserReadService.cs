using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.DTO.DTOs;
using humanResourceProject.DTO.VMs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Application.Services.Abstract.IAppUserServices
{
    public interface IAppUserReadService : IBaseReadService<AppUser>
    {
        Task<SignInResult> Login(LoginDTO model);
        Task Logout();
        Task<List<PersonelVM>> GetEmployeesByCompanyId(Guid companyId);
    }
}
