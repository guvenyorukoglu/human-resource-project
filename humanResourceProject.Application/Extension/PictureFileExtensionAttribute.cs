using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Application.Extension
{
    public class PictureFileExtensionAttribute :ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            IFormFile file = (IFormFile)value;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                string[] extensions = { "jpg", "png", "jpeg" };
                bool result = extensions.Any(x => x.EndsWith(extension));

                if (!result)
                    return new ValidationResult("Valid formats are 'png', 'jpg','jpeg'");
            }
            return ValidationResult.Success;
        }
    }
}
