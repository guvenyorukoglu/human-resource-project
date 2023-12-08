using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;

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
