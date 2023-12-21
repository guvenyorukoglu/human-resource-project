using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Presentation.Controllers;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Abstract.IAppUserServices
{
    public interface IAppUserWriteService : IBaseWriteService<AppUser>
    {
        Task<IdentityResult> RegisterPersonelManager(CreateEmployeeDTO model); // CreateEmployeeDTO modelini alır, veritabanına PersonelManager olarak kaydeder.

        Task<IdentityResult> RegisterPersonel(CreateEmployeeDTO model); // UserRegisterDTO modelini alır, veritabanına Personel olarak kaydeder.
        Task<IdentityResult> RegisterCompanyManager(CompanyManagerRegisterDTO model); // UserRegisterDTO modelini alır, veritabanına Şirket Yöneticisi olarak kaydeder.
        Task<UpdateUserDTO> GetUpdateUserDTOById(Guid id); // Güncelleme sayfası için UpdateUserDTO döndürür.
        Task<IdentityResult> Update(UpdateUserDTO model); // UpdateUserDTO modelini alır, veritabanında günceller.
        Task<IdentityResult> UpdateProfileImage(Guid id); // Profil resmini günceller.

    }
}
