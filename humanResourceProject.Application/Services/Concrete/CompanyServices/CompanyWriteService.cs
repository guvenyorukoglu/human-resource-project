using AutoMapper;
using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.DataProtection;

namespace humanResourceProject.Application.Services.Concrete.CompanyServices
{
    public class CompanyWriteService : BaseWriteService<Company>, ICompanyWriteService
    {
        private readonly IBaseWriteRepository<Company> _writeRepository;
        private readonly IMapper _mapper;
        public CompanyWriteService(IBaseWriteRepository<Company> writeRepository, IBaseReadRepository<Company> readRepository, IMapper mapper) : base(writeRepository, readRepository)
        {
            _writeRepository = writeRepository;
            _mapper = mapper;
        }

        public Task Register(CompanyRegisterDTO model)
        {
            //ToDo : Mapping
            throw new NotImplementedException();
        }
        public async Task<bool> Create(CompanyRegisterDTO model)
        {
            if (model == null)
                return false;
            else
            {
                Company CreateCompany = _mapper.Map<Company>(model);
                await _writeRepository.Insert(CreateCompany);
                return true;

            }

        }
        public async Task Update(UpdateCompanyDTO model)
        {
            Company UpdateCompany = _mapper.Map<Company>(model);
     
                await _writeRepository.Update(UpdateCompany);
        }
    }
}
