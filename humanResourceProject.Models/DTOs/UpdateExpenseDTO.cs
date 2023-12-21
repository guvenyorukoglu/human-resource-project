using humanResourceProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using humanResourceProject.Domain.Entities.Concrete;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateExpenseDTO
    {


        public Guid Id { get; set; }

        [Required(ErrorMessage = "Açıklama alanı boş geçilemez!")]
        [DisplayName("Açıklama*")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string? Explanation { get; set; }
        [Required(ErrorMessage = "Miktar alanı boş geçilemez!")]
        [DisplayName("Harcama Miktarı*")]
        public decimal AmountOfExpense { get; set; }
        public DateTime? UpdateDate { get; set; }=DateTime.Now;
        public Status Status { get; set; } = Status.Active;
        public RequestStatus ExpenseStatus { get; set; } = RequestStatus.Pending;
      

    }
}
