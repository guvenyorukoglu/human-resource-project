using humanResourceProject.Models.DTOs;
using humanResourceProject.Presentation.Controllers;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Abstract.IImageServices
{
    public interface IImageService
    {
        Task<UserRegisterDTO> UploadImageToAzure(UserRegisterDTO model);
        Task<ExpenseDTO> UploadExpenseImageToAzure(ExpenseDTO model);
        Task<IdentityResult> UpdateProfileImage(UpdateProfileImageDTO model); // Profil resmini günceller.
    }
}
