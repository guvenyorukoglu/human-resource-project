using humanResourceProject.Domain.Enum;
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
        [StringLength(20, ErrorMessage = "İsim en fazla 20 en az 3 karakter olmalıdır.", MinimumLength = 3)]
        public string FirstName { get; set; }

        [DisplayName("İkinci İsim")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        [StringLength(30, ErrorMessage = "İsim en fazla 30 karakter olmalıdır.", MinimumLength = 3)]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Soyisim alanı boş geçilemez!")]
        [DisplayName("Soyisim*")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        [StringLength(30, ErrorMessage = "İsim en fazla 30 en az 5 karakter olmalıdır.", MinimumLength = 5)]
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

        [Required(ErrorMessage = "Telefon numarası alanı boş geçilemez!")]
        [DisplayName("Telefon Numarası*")]
        [PhoneValidations(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Doğum tarihi alanı boş geçilemez!")]
        [DisplayName("Doğum Tarihi*")]
        [BirthdateValidations(ErrorMessage = "18-85 yaş aralığında bir doğum tarihi giriniz!")]
        public DateTime Birthdate { get; set; }

        // public DateTime DateOfEmployment { get; set; }

        [Required(ErrorMessage = "Adres alanı boş geçilemez!")]
        [DisplayName("Adres*")]
        [StringLength(150, ErrorMessage = "Adres en fazla 150 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Address { get; set; }

        [Required(ErrorMessage = "TC kimlik numarası alanı boş geçilemez!")]
        [DisplayName("TC Kimlik Numarası*")]
        [IdentityNumberValidations(ErrorMessage = "Lütfen geçerli bir TC kimlik numarası giriniz!")]
        public string IdentificationNumber { get; set; }

        [Required(ErrorMessage = "Kan grubu alanı boş geçilemez!")]
        [DisplayName("Kan Grubu*")]
        public BloodGroup BloodGroup { get; set; }

        [Required(ErrorMessage = "Cinsiyet alanı boş geçilemez!")]
        [DisplayName("Cinsiyet*")]
        public Gender Gender { get; set; }

        public string? ImagePath { get; set; }

        [DisplayName("Profil Fotoğrafı")]
        public IFormFile? UploadPath { get; set; }

        public Guid? ManagerId { get; set; }
        public Guid JobId { get; set; } = Guid.Empty;
        public Guid DepartmentId { get; set; } = Guid.Empty;
        public Guid CompanyId { get; set; }



    }
}
