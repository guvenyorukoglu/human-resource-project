using humanResourceProject.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Models.DTOs
{
    public class ResetPasswordDTO
    {


    
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [PasswordValidations(ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakterden oluşmalıdır.")]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrarı")]
        [Compare("Password", ErrorMessage = "Bir önceki girilen şifreyle aynı olmalıdır!")]

        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
