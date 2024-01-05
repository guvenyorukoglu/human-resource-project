using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.DTOs
{
    public class JobDTO
    {
        [Required(ErrorMessage = "Pozisyon adı alanı boş geçilemez!")]
        [DisplayName("Pozisyon Adı*")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ\s]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        public string Title { get; set; }

        [DisplayName("Pozisyon Tanımı")]
        [StringLength(150, ErrorMessage = "Pozisyon tanımı en fazla 150 karakter olmalıdır.")]
        public string? Description { get; set; }
        public Guid CompanyId { get; set; }
    }
}
