using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace humanResourceProject.Models.DTOs
{
    public class ExpenseDTO
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez!")]
        [DisplayName("Açıklama*")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Miktar alanı boş geçilemez!")]
        [DisplayName("Harcama Miktarı*")]
        public decimal AmountOfExpense { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Status Status { get; set; }

    }
}
