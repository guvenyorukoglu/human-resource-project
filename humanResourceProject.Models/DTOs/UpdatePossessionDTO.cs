using humanResourceProject.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace humanResourceProject.Models.DTOs
{
    public class UpdatePossessionDTO
    {
        public Guid Id { get; set; }
        //TODO: Add validation for barcode
        [Required(ErrorMessage = "Barkod alanı boş geçilemez!")]
        [DisplayName("Barkod*")]
        public string Barcode { get; set; }
        public Guid CompanyId { get; set; }

        [Required(ErrorMessage = "Marka adı alanı boş geçilemez!")]
        [DisplayName("Marka Adı*")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model adı alanı boş geçilemez!")]
        [DisplayName("Model Adı*")]
        public string PossessionModel { get; set; }

        [DisplayName("Detaylar")]
        [StringLength(500, ErrorMessage = "Detaylar alanı en fazla 500 karakter olmalıdır.")]
        public string? Details { get; set; }
        public PossessionType PossessionType { get; set; }
    }
}
