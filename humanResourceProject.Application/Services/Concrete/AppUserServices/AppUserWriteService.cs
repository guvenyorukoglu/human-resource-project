using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

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
