using humanResourceProject.Domain.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateDepartmentDTO
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Departman adı alanı boş geçilemez!")]
        [DisplayName("Departman adı*")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        public string DepartmentName { get; set; }

        [DisplayName("Açıklama")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string? Description { get; set; }
        public Status Status { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
