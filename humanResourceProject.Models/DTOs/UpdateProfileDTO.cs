using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateProfileDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Kan grubu alanı boş geçilemez!")]
        [DisplayName("Kan Grubu*")]
        public BloodGroup BloodGroup { get; set; }


        [Required(ErrorMessage = "Adres alanı boş geçilemez!")]
        [DisplayName("Adres*")]
        [StringLength(150, ErrorMessage = "Adres en fazla 200 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telefonu alanı boş geçilemez!")]
        [DisplayName("Telefon*")]
        [PhoneValidations(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        public string PhoneNumber { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? JobTitle { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public Gender Gender { get; set; }
        public string? ManagerName { get; set; }
        public string? ManagerEmail { get; set; }
        public string? ManagerPhoneNumber { get; set; }
    }
}
