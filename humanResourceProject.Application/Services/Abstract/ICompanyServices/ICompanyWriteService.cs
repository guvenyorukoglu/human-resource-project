using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;

namespace humanResourceProject.Application.Services.Abstract.ICompanyServices
{
    public interface ICompanyWriteService : IBaseWriteService<Company>
    {
        Task Register(CompanyRegisterDTO model);
        Task<bool> Create(CompanyRegisterDTO model);
        Task Update(UpdateCompanyDTO model);
    }
}
