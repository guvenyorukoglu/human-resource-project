using humanResourceProject.Models.DTOs;
using humanResourceProject.Presentation.Controllers;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Abstract.IImageServices
{
    public interface IImageService
    {
        Task<UserRegisterDTO> UploadImageToAzure(UserRegisterDTO model);
        Task<ExpenseDTO> UploadExpenseImageToAzure(ExpenseDTO model); // Gider resmini günceller.
        Task<IdentityResult> UpdateProfileImage(UpdateProfileImageDTO model); // Profil resmini günceller.
        Task<UpdateExpenseDTO> UploadExpenseImageToAzure(UpdateExpenseDTO model);
    }
}
