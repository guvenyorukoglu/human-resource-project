using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateUserDTO
    {
        [Required(ErrorMessage = "Email alanı boş geçilemez!")]
        [DisplayName("Email")]
        [EmailValidations(ErrorMessage = "Lütfen geçerli bir email adresi giriniz!")]
        public string Email { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        [Required(ErrorMessage = "Telefonu alanı boş geçilemez!")]
        [DisplayName("Telefon")]
        [PhoneValidations(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        public string PhoneNumber { get; set; }
        public Title Title { get; set; }
        public string Job { get; set; }
        public DateTime UpdateDate { get; set; }
        public Status status { get; set; }
    }
}
