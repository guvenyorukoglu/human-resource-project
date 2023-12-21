using humanResourceProject.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.DTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [PasswordValidations(ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakterden oluşmalıdır.")]
        [Display(Name = "Yeni Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre Tekrarı")]
        [Compare("Password", ErrorMessage = "Bir önceki girilen şifreyle aynı olmalıdır!")]

        public string ConfirmPassword { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
