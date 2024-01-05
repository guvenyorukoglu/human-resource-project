using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateJobDTO
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Pozisyon adı alanı boş geçilemez!")]
        [DisplayName("Pozisyon Adı*")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ\s]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        [StringLength(50, ErrorMessage = "Pozisyon tanımı en fazla 50 karakter olmalıdır.")]
        public string Title { get; set; }

        [DisplayName("Pozisyon Tanımı")]
        [StringLength(500, ErrorMessage = "Pozisyon tanımı en fazla 500 karakter olmalıdır.")]
        public string? Description { get; set; }
        public Guid CompanyId { get; set; }
    }
}
