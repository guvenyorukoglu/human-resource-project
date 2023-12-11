using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "İsim alanı boş geçilemez!")]
        [DisplayName("İsim")]
        public string FirstName { get; set; }
        [DisplayName("İkinci İsim")]
        public string? MiddleName { get; set; }
        [Required(ErrorMessage = "Soyisim alanı boş geçilemez!")]
        [DisplayName("Soyisim")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email alanı boş geçilemez!")]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre alanı boş geçilemez!")]
        [DisplayName("Şifre")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Şifre alanı boş geçilemez!")]
        [DisplayName("Şifreyi Onaylayın")]
        [Compare("Password", ErrorMessage ="Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Telefonu alanı boş geçilemez!")]
        [DisplayName("Telefon")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Doğum tarihi boş geçilemez!")]
        [DisplayName("Doğum Tarihi")]
        public DateTime Birthdate { get; set; }

        // public DateTime DateOfEmployment { get; set; }

        [Required(ErrorMessage = "Adres alanı boş geçilemez!")]
        [DisplayName("Adres")]
        [StringLength(200, ErrorMessage = "Adres en fazla 200 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Address { get; set; }
        [Required(ErrorMessage = "TC No alanı boş geçilemez!")]
        [DisplayName("TC NO")]
        public string IdentificationNumber { get; set; }
        [Required(ErrorMessage = "Kan Grubu alanı boş geçilemez!")]
        [DisplayName("Kan Grubu")]
        public BloodGroup BloodGroup { get; set; }
        [Required(ErrorMessage = "Ünvan alanı boş geçilemez!")]
        [DisplayName("Ünvan")]
        public Title Title { get; set; }
        [Required(ErrorMessage = "Meslek alanı boş geçilemez!")]
        [DisplayName("Meslek")]
        public string Job { get; set; }
        public string? ImagePath { get; set; }

        [DisplayName("Profil Fotoğrafı")]
        public IFormFile? UploadPath { get; set; }
        [Required(ErrorMessage = "Şirket alanı boş geçilemez!")]
        [DisplayName("Şirket")]
        public Guid CompanyId { get; set; }
        public List<CompanyVM>? Companies { get; set; }


    }
}
