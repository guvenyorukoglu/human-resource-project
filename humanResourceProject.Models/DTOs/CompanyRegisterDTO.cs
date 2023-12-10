using humanResourceProject.Models.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.DTOs
{
    public class CompanyRegisterDTO
    {
        [Required(ErrorMessage = "Şirket Adı alanı boş geçilemez!")]
        [DisplayName("Şirket Adı")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Adres alanı boş geçilemez!")]
        [DisplayName("Adres")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vergi numarası alanı boş geçilemez!")]
        [DisplayName("Vergi Numarası")]
        [TaxNumberValidations(ErrorMessage = "Lütfen geçerli bir vergi numarası giriniz!")]
        public string TaxNumber { get; set; }
        [Required(ErrorMessage = "Vergi dairesi alanı boş geçilemez!")]
        [DisplayName("Vergi Dairesi")]
        public string TaxOffice { get; set; }
        [Required(ErrorMessage = "Telefonu alanı boş geçilemez!")]
        [DisplayName("Telefon")]
        [PhoneValidations(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Personel sayısı alanı boş geçilemez!")]
        [DisplayName("Personel Sayısı")]
        public int NumberOfEmployees { get; set; }

    }
}
