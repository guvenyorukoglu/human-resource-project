using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.VMs;
using humanResourceProject.Models.Validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.DTOs
{
    public class UserRegisterDTO
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
        [Required(ErrorMessage = "Şifre alanı boş geçilemez!")]
        [DisplayName("Şifre*")]
        [PasswordValidations(ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakterden oluşmalıdır.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı alanı boş geçilemez!")]
        [DisplayName("Şifre Tekrarı*")]
        [Compare("Password", ErrorMessage = "Bir önceki girilen şifreyle aynı olmalıdır!")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Telefonu alanı boş geçilemez!")]
        [DisplayName("Telefon*")]
        [PhoneValidations(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Doğum tarihi boş geçilemez!")]
        [DisplayName("Doğum Tarihi*")]
        [BirthdateValidations(ErrorMessage ="18 yaşından küçük olamazsınız!")]
        public DateTime Birthdate { get; set; }

        // public DateTime DateOfEmployment { get; set; }

        [Required(ErrorMessage = "Adres alanı boş geçilemez!")]
        [DisplayName("Adres*")]
        [StringLength(200, ErrorMessage = "Adres en fazla 200 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Address { get; set; }
        [Required(ErrorMessage = "TC No alanı boş geçilemez!")]
        [DisplayName("TC Kimlik Numarası*")]
        //[IdentityNumberValidations(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        public string IdentificationNumber { get; set; }
        [Required(ErrorMessage = "Kan Grubu alanı boş geçilemez!")]
        [DisplayName("Kan Grubu*")]
        public BloodGroup BloodGroup { get; set; }
        [Required(ErrorMessage = "Ünvan alanı boş geçilemez!")]
        [DisplayName("Ünvan*")]
        public Title Title { get; set; }
        [Required(ErrorMessage = "Meslek alanı boş geçilemez!")]
        [DisplayName("Meslek*")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ\s]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        public string Job { get; set; }
        public string? ImagePath { get; set; }

        [DisplayName("Profil Fotoğrafı")]
        public IFormFile? UploadPath { get; set; }
        [Required(ErrorMessage = "Şirket alanı boş geçilemez!")]
        [DisplayName("Şirket")]
        public Guid CompanyId { get; set; }
        //public List<CompanyVM>? Companies { get; set; }


    }
}
