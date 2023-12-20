using humanResourceProject.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace humanResourceProject.Models.DTOs
{
    public class DepartmentDTO
    {
        [Required(ErrorMessage = "Departman adı alanı boş geçilemez!")]
        [DisplayName("Departman adı*")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ\s]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        public string DepartmentName { get; set; }

        [DisplayName("Açıklama")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olmalıdır.")]
        public string? Description { get; set; }

        public Guid CompanyId { get; set; }
    }
}
