using AutoMapper;
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
        private readonly IMapper _mapper;
        public AppUserWriteService(IBaseWriteRepository<AppUser> writeRepository, IBaseReadRepository<AppUser> readRepository, IMapper mapper) : base(writeRepository, readRepository)
        {
            _writeRepository = writeRepository;
            _mapper = mapper;
        }

        public Task<IdentityResult> Register(UserRegisterDTO model)
        {
            //ToDo : Mapping 
            throw new NotImplementedException();
        }
        public async Task<bool> Create(UserRegisterDTO model)
        {
            if (model == null)
                return false;
            else
            {
                AppUser CreateUser = _mapper.Map<AppUser>(model);
                await _writeRepository.Insert(CreateUser);
                return true;
            }
        }
        public async Task Update(UpdateUserDTO model)
        {
            AppUser CreateUser = _mapper.Map<AppUser>(model);
            await _writeRepository.Update(CreateUser);
        }


    }
}
