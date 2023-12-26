using humanResourceProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Collections.Specialized.BitVector32;
using System.Reflection.Emit;

namespace humanResourceProject.Models.VMs
{
    public class AdvancePersonnelVM
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        [Required(ErrorMessage = "Miktar alanı boş geçilemez!")]
        [DisplayName("Avans Miktarı*")]
        public decimal AmountOfAdvance { get; set; }
        [Required(ErrorMessage = "Avans tarihi boş geçilemez!")]
        [DisplayName("Son İstenen Tarihi*")]
        public DateTime ExpiryDate { get; set; }
        public Status Status { get; set; }
        [Required(ErrorMessage = "Avans tipi alanı boş geçilemez!")]
        [DisplayName("Avans Tipi*")]
        public AdvanceType AdvanceType { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez!")]
        [DisplayName("Açıklama*")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Explanation { get; set; }
        public RequestStatus AdvanceStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public Currency Currency { get; set; }
        public string ManagerFullName { get; set; }
        public string AdvanceNo { get; set; }
    }
}
