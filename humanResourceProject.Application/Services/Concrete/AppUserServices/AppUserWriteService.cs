using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.DTO.DTOs;
using humanResourceProject.Infrastructure.Repositories.AppUserRepos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Application.Services.Concrete.AppUserServices
{
    public class AppUserWriteService : BaseWriteService<AppUser>, IAppUserWriteService
    {
        private readonly IBaseWriteRepository<AppUser> _writeRepository;
        public AppUserWriteService(IBaseWriteRepository<AppUser> writeRepository, IBaseReadRepository<AppUser> readRepository) : base(writeRepository, readRepository)
        {
        }

        public Task<IdentityResult> Register(UserRegisterDTO model)
        {
            //ToDo : Mapping 
            throw new NotImplementedException();
        }
    }
}
