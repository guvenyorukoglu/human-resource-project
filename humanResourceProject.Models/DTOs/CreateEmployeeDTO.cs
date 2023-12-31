using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.Validations;
using humanResourceProject.Models.VMs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace humanResourceProject.Models.DTOs
{
    public class CreateEmployeeDTO
    {
        [Required(ErrorMessage = "İsim alanı boş geçilemez!")]
        [DisplayName("İsim*")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        public string FirstName { get; set; }

        [DisplayName("İkinci İsim")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Soyisim alanı boş geçilemez!")]
        [DisplayName("Soyisim*")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email alanı boş geçilemez!")]
        [DisplayName("Email*")]
        [EmailValidations(ErrorMessage = "Lütfen geçerli bir email adresi giriniz!")]
        public string Email { get; set; }
        public string Password { get; set; } = "Aa123+";

        public string ConfirmPassword { get; set; } = "Aa123+";

        [Required(ErrorMessage = "Telefonu alanı boş geçilemez!")]
        [DisplayName("Telefon*")]
        [PhoneValidations(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Doğum tarihi boş geçilemez!")]
        [DisplayName("Doğum Tarihi*")]
        [BirthdateValidations(ErrorMessage = "18-85 yaş aralığında bir doğum tarihi giriniz!")]
        public DateTime Birthdate { get; set; }

        // public DateTime DateOfEmployment { get; set; }

        [Required(ErrorMessage = "Adres alanı boş geçilemez!")]
        [DisplayName("Adres*")]
        [StringLength(200, ErrorMessage = "Adres en fazla 200 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Address { get; set; }

        [Required(ErrorMessage = "TC No alanı boş geçilemez!")]
        [DisplayName("TC Kimlik Numarası*")]
        [IdentityNumberValidations(ErrorMessage = "Lütfen geçerli bir TC kimlik numarası giriniz!")]
        public string IdentificationNumber { get; set; }

        [Required(ErrorMessage = "Kan Grubu alanı boş geçilemez!")]
        [DisplayName("Kan Grubu*")]
        public BloodGroup BloodGroup { get; set; }

        [Required(ErrorMessage = "Cinsiyet alanı boş geçilemez!")]
        [DisplayName("Cinsiyet*")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Pozisyon alanı boş geçilemez!")]
        [DisplayName("Pozisyon*")]
        public Guid JobId { get; set; }

        public List<JobVM>? Jobs { get; set; }

        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "Departman alanı boş geçilemez!")]
        [DisplayName("Departman*")]
        public Guid DepartmentId { get; set; }
        public List<DepartmentVM>? Departments { get; set; }

        [Required(ErrorMessage = "Yönetici alanı boş geçilemez!")]
        [DisplayName("Yönetici*")]
        public Guid ManagerId { get; set; }
        public List<ManagerVM>? Managers { get; set; }
        public Guid CompanyId { get; set; }
    }
}
