using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateDepartmentDTO
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Departman adı alanı boş geçilemez!")]
        [DisplayName("Departman Adı*")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ\s]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        public string DepartmentName { get; set; }

        [DisplayName("Departman Tanımı")]
        [StringLength(200, ErrorMessage = "Departman tanımı en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string? Description { get; set; }
        public Guid CompanyId { get; set; }
    }
}
