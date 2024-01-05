using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Abstract.IAppUserServices
{
    public interface IAppUserWriteService : IBaseWriteService<AppUser>
    {
        Task<IdentityResult> RegisterPersonelManager(CreateEmployeeDTO model); // CreateEmployeeDTO modelini alır, veritabanına PersonelManager olarak kaydeder.

        Task<IdentityResult> RegisterPersonel(CreateEmployeeDTO model); // UserRegisterDTO modelini alır, veritabanına Personel olarak kaydeder.
        Task<IdentityResult> RegisterCompanyManager(UserRegisterDTO model); // UserRegisterDTO modelini alır, veritabanına Şirket Yöneticisi olarak kaydeder.
        Task<UpdateUserDTO> GetUpdateUserDTOById(Guid id); // Güncelleme sayfası için UpdateUserDTO döndürür.
        Task<UpdateProfileDTO> GetUpdateProfileDTOById(Guid id); // Güncelleme sayfası için UpdateUserDTO döndürür.
        Task<IdentityResult> Update(UpdateUserDTO model); // UpdateUserDTO modelini alır, veritabanında günceller.
        Task<IdentityResult> UpdateProfile(UpdateProfileDTO model); // UpdateUserDTO modelini alır, veritabanında günceller.

        Task<IdentityResult> UpdateProfileImage(Guid id); // Profil resmini günceller.
        Task<IdentityResult> FireEmployee(FireEmployeeDTO model); // Personelin statüsünü inactive olarak günceller.
        Task<IdentityResult> DeleteEmployee(Guid id); // Personelin statüsünü silinmiş olarak günceller.
    }
}
