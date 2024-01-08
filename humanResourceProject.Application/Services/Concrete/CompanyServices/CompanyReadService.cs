using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace humanResourceProject.Application.Services.Concrete.CompanyServices
{
    public class CompanyReadService : BaseReadService<Company>, ICompanyReadService
    {
        private readonly IBaseReadRepository<Company> _readRepository;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public CompanyReadService(IBaseReadRepository<Company> readRepository, RoleManager<IdentityRole<Guid>> roleManager) : base(readRepository)
        {
            _readRepository = readRepository;
            _roleManager = roleManager;
        }
       
        public async Task<CompanyVM> GetCompanyVM(Guid id)
        {
          
            return await _readRepository.GetFilteredFirstOrDefault(
                select: x => new CompanyVM()
                {
                    Address = x.Address,
                    CompanyName = x.CompanyName,
                    PhoneNumber = x.PhoneNumber,
                    TaxNumber = x.TaxNumber,
                    TaxOffice = x.TaxOffice,
                    FirstName = x.Employees.FirstOrDefault(a => a.CompanyId == x.Id  ).FirstName ,
                    //LastName = x.Employees.FirstOrDefault(a => a.Company.CompanyName == x.CompanyName).LastName

                },
                where: x => x.Id == id && (x.Status != Domain.Enum.Status.Inactive && x.Status != Domain.Enum.Status.Deleted),
                include: x => x.Include(x => x.Employees));
                

        }
    }
}
