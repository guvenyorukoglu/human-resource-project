using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Concrete.CompanyServices
{
    public class CompanyReadService : BaseReadService<Company>, ICompanyReadService
    {
        private readonly IBaseReadRepository<Company> _readRepository;
        public CompanyReadService(IBaseReadRepository<Company> readRepository) : base(readRepository)
        {
            _readRepository = readRepository;
        }

        public Task<List<CompanyVM>> GetCompanies()
        {
            return _readRepository.GetFilteredList(
                               select: x => new CompanyVM()
                               {
                                   Id = x.Id,
                                   Address = x.Address,
                                   CompanyName = x.CompanyName,
                                   PhoneNumber = x.PhoneNumber,
                                   TaxNumber = x.TaxNumber,
                                   TaxOffice = x.TaxOffice,
                                   NumberOfEmployees = x.NumberOfEmployees,
                                   Status = x.Status,
                                   CompanyStatus = x.CompanyStatus,
                                   ContactFullName = x.ContactFullName,
                                   ContactEmail = x.ContactEmail,
                                   ContactPhoneNumber = x.ContactPhoneNumber,
                               },
                               where: x => x.CompanyName != "MonitorEase" && (x.Status != Domain.Enum.Status.Deleted && x.Status != Domain.Enum.Status.Inactive));
        }

        public async Task<CompanyVM> GetCompanyVM(Guid id)
        {

            return await _readRepository.GetFilteredFirstOrDefault(
                select: x => new CompanyVM()
                {
                    Id = x.Id,
                    Address = x.Address,
                    CompanyName = x.CompanyName,
                    PhoneNumber = x.PhoneNumber,
                    TaxNumber = x.TaxNumber,
                    TaxOffice = x.TaxOffice,
                    NumberOfEmployees = x.NumberOfEmployees,
                    Status = x.Status,
                    CompanyStatus = x.CompanyStatus,
                    ContactFullName = x.ContactFullName,
                    ContactEmail = x.ContactEmail,
                    ContactPhoneNumber = x.ContactPhoneNumber,
                },
                where: x => x.Id == id && (x.Status != Domain.Enum.Status.Inactive && x.Status != Domain.Enum.Status.Deleted));


        }
    }
}
