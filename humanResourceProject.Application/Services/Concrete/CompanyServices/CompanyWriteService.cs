using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.DTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Application.Services.Concrete.CompanyServices
{
    public class CompanyWriteService : BaseWriteService<Company>, ICompanyWriteService
    {
        private readonly IBaseWriteRepository<Company> _writeRepository;
        public CompanyWriteService(IBaseWriteRepository<Company> writeRepository, IBaseReadRepository<Company> readRepository) : base(writeRepository, readRepository)
        {
            _writeRepository = writeRepository;
        }

        public Task Register(CompanyRegisterDTO model)
        {
            //ToDo : Mapping
            throw new NotImplementedException();
        }
    }
}
