using humanResourceProject.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace humanResourceProject.Models.DTOs
{
    public class PossessionDTO
    {

        [Required(ErrorMessage = "Barkod alanı boş geçilemez!")]
        [DisplayName("Barkod*")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "Barkod numarası 13 rakamdan oluşmalıdır.")]
        public string Barcode { get; set; }
        public Guid CompanyId { get; set; }

        [Required(ErrorMessage = "Marka adı alanı boş geçilemez!")]
        [DisplayName("Marka Adı*")]
        [StringLength(50, ErrorMessage = "Marka adı  en fazla 50 karakter olmalıdır.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model adı alanı boş geçilemez!")]
        [DisplayName("Model Adı*")]
        [StringLength(50, ErrorMessage = "Model adı  en fazla 50 karakter olmalıdır.")]
        public string PossessionModel { get; set; }

        [DisplayName("Detaylar")]
        [StringLength(250, ErrorMessage = "Detaylar alanı en fazla 250 karakter olmalıdır.")]

        public string? Details { get; set; }
        [Required(ErrorMessage = "Zimmet türü alanı boş geçilemez!")]
        [DisplayName("Zimmet Türü*")]
        public PossessionType PossessionType { get; set; }
    }
}
