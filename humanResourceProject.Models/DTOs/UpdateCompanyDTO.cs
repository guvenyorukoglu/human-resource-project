using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace humanResourceProject.Models.DTOs
{
    public class UpdateCompanyDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Şirket Adı alanı boş geçilemez!")]
        [DisplayName("Şirket Adı*")]
        [StringLength(100, ErrorMessage = "Şirket adı en fazla 100 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Adres alanı boş geçilemez!")]
        [DisplayName("Adres*")]
        [StringLength(200, ErrorMessage = "Adres en fazla 200 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Address { get; set; }
        [Required(ErrorMessage = "Telefonu alanı boş geçilemez!")]
        [DisplayName("Telefon*")]
        [PhoneValidations(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Personel sayısı alanı boş geçilemez!")]
        [DisplayName("Personel Sayısı*")]
        public int NumberOfEmployees { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Status Status { get; set; }
        public string? RejectReason { get; set; }
        public RequestStatus CompanyStatus { get; set; }

    }
}
