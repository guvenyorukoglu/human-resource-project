using AutoMapper;
using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Concrete.CompanyServices
{
    public class CompanyWriteService : BaseWriteService<Company>, ICompanyWriteService
    {
        private readonly IBaseWriteRepository<Company> _writeRepository;
        private readonly IBaseReadRepository<Company> _readRepository;
        private readonly IMapper _mapper;
        public CompanyWriteService(IBaseWriteRepository<Company> writeRepository, IBaseReadRepository<Company> readRepository, IMapper mapper) : base(writeRepository, readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
        }

        public async Task<UpdateCompanyDTO> GetUpdateCompanyDTOById(Guid id)
        {
            Company company = await _readRepository.GetSingleDefault(x => x.Id == id);
            if (company == null)
                return null;

            UpdateCompanyDTO updateCompanyDTO = _mapper.Map<UpdateCompanyDTO>(company);
            return updateCompanyDTO;
        }

        public async Task<IdentityResult> RegisterCompany(CompanyRegisterDTO model)
        {
            if (model == null)
                return IdentityResult.Failed();

            Company newCompany = _mapper.Map<Company>(model);
            
            if (await _writeRepository.Insert(newCompany))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }
        public async Task<IdentityResult> Update(UpdateCompanyDTO model)
        {
            Company updatedCompany = await _readRepository.GetSingleDefault(x => x.Id == model.Id);
            if (updatedCompany == null)
                return IdentityResult.Failed();

            updatedCompany.CompanyName = model.CompanyName ?? updatedCompany.CompanyName;
            updatedCompany.Address = model.Address ?? updatedCompany.Address;
            updatedCompany.PhoneNumber = model.PhoneNumber ?? updatedCompany.PhoneNumber;
            updatedCompany.NumberOfEmployees = model.NumberOfEmployees;
            updatedCompany.UpdateDate = DateTime.Now;
            updatedCompany.Status = Status.Modified;

            if(await _writeRepository.Update(updatedCompany))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }
    }
}
