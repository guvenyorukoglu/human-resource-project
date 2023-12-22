using Microsoft.AspNetCore.Http;

namespace humanResourceProject.Presentation.Controllers
{
    public class UpdateProfileImageDTO
    {
        public Guid Id { get; set; }
        //public string? ImagePath { get; set; }
        public IFormFile UploadPath { get; set; }
    }
}