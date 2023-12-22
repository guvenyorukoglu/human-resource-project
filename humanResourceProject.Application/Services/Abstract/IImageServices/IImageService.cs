using humanResourceProject.Models.DTOs;
using humanResourceProject.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Abstract.IImageServices
{
    public interface IImageService
    {
        Task<CompanyManagerRegisterDTO> UploadImageToAzure(CompanyManagerRegisterDTO model);
        Task<ExpenseDTO> UploadExpenseImageToAzure(ExpenseDTO model);
        Task<IdentityResult> UpdateProfileImage(UpdateProfileImageDTO model); // Profil resmini günceller.
    }
}
