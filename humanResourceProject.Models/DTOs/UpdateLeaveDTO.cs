using humanResourceProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateLeaveDTO
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        [Required(ErrorMessage = "İzin tipi alanı boş geçilemez!")]
        [DisplayName("İzin Tipi*")]
        public LeaveType LeaveType { get; set; }
        [Required(ErrorMessage = "İzin başlangıç tarihi boş geçilemez!")]
        [DisplayName("İzin Başlangıç Tarihi*")]
        public DateTime StartDateOfLeave { get; set; }
        [Required(ErrorMessage = "İzin bitiş tarihi boş geçilemez!")]
        [DisplayName("İzin Bitiş Tarihi*")]
        public DateTime EndDateOfLeave { get; set; }
        public RequestStatus LeaveStatus { get; set; }
        [Required(ErrorMessage = "İzin gün miktarı boş geçilemez!")]
        [DisplayName("İzinli Gün Miktarı*")]
        public decimal DaysOfLeave { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez!")]
        [DisplayName("Açıklama*")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Explanation { get; set; }
    }
}
