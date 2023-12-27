using humanResourceProject.Models.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.DTOs
{
    public class CompanyRegisterDTO
    {
        [Required(ErrorMessage = "Şirket adı alanı boş geçilemez!")]
        [DisplayName("Şirket Adı*")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Şirket adresi alanı boş geçilemez!")]
        [DisplayName("Şirket Adresi*")]
        [StringLength(200, ErrorMessage = "Adres alanı en fazla 200, en az 3 karakter olabilir!", MinimumLength = 3)]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vergi numarası alanı boş geçilemez!")]
        [DisplayName("Vergi Numarası*")]
        [TaxNumberValidations(ErrorMessage = "Lütfen geçerli bir vergi numarası giriniz!")]
        public string TaxNumber { get; set; }
        [Required(ErrorMessage = "Vergi dairesi alanı boş geçilemez!")]
        [DisplayName("Vergi Dairesi*")]
        [StringLength(50, ErrorMessage = "Vergi dairesi alanı en fazla 50, en az 3 karakter olabilir!", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        public string TaxOffice { get; set; }
        [Required(ErrorMessage = "Şirket telefon numarası alanı boş geçilemez!")]
        [DisplayName("Şirket Telefon Numarası*")]
        [PhoneValidations(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Personel sayısı alanı boş geçilemez!")]
        [DisplayName("Personel Sayısı*")]
        [NumberOfEmployeesValidation(ErrorMessage = "Lütfen geçerli bir personel sayısı giriniz!")]
        public int NumberOfEmployees { get; set; }

    }
}
