using humanResourceProject.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace humanResourceProject.Models.DTOs
{
    public class DepartmentDTO
    {
        [Required(ErrorMessage = "Departman adı alanı boş geçilemez!")]
        [DisplayName("Departman Adı*")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ\s]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        public string DepartmentName { get; set; }

        [DisplayName("Departman Tanımı")]
        [StringLength(500, ErrorMessage = "Departman tanımı en fazla 500 karakter olmalıdır.")]
        public string? Description { get; set; }

        public Guid CompanyId { get; set; }
    }
}
