using humanResourceProject.Models.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace humanResourceProject.Models.DTOs
{
    public class LoginDTO
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Email alanı boş geçilemez!")]
        [DisplayName("Email")]
        [EmailValidations(ErrorMessage = "Lütfen geçerli bir email adresi giriniz!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre alanı boş geçilemez!")]
        [DisplayName("Şifre")]
        [PasswordValidations(ErrorMessage = "Lütfen geçerli bir şifre belirleyin!")]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
