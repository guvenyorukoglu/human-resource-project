using humanResourceProject.Models.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace humanResourceProject.Models.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "Email alanı boş geçilemez!")]
        [DisplayName("Email*")]
        [EmailValidations(ErrorMessage = "Lütfen geçerli bir email adresi giriniz!")]
        public string Email { get; set; }
    }
}
