using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Abstract.ICompanyServices
{
    public interface ICompanyWriteService : IBaseWriteService<Company>
    {
        Task<IdentityResult> RegisterCompany(CompanyRegisterDTO model); // CompanyRegisterDTO modelini alır, veritabanına kaydeder.
        Task<UpdateCompanyDTO> GetUpdateCompanyDTOById(Guid id); // Güncelleme sayfası için UpdateCompanyDTO döndürür.
        Task<IdentityResult> Update(UpdateCompanyDTO model); // UpdateCompanyDTO modelini alır, veritabanında günceller.
    }
}
