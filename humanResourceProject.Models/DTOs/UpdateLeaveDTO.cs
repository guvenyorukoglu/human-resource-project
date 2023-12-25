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
        [Required(ErrorMessage = "İzin tipi boş geçilemez!")]
        [DisplayName("İzin tipi*")]
        public LeaveType LeaveType { get; set; }
        [Required(ErrorMessage = "İzin başlangıç tarihi boş geçilemez!")]
        [DisplayName("İzin başlangıç Tarihi*")]
        public DateTime StartDateOfLeave { get; set; }
        [Required(ErrorMessage = "İzin bitiş tarihi boş geçilemez!")]
        [DisplayName("İzin bitiş Tarihi*")]
        public DateTime EndDateOfLeave { get; set; }
        public Guid EmployeeId { get; set; }
        //public Guid ManagerId { get; set; }
        //public Status Status { get; set; } = Status.Modified;
        //public DateTime? UpdateDate { get; set; } = DateTime.Now;
        public RequestStatus LeaveStatus { get; set; }
        [Required(ErrorMessage = "İzin süresi boş geçilemez!")]
        [DisplayName("İzin süresi*")]
        public decimal DaysOfLeave { get; set; }
        
        [Required(ErrorMessage = "İzin açıklaması boş geçilemez!")]
        [DisplayName("İzin Açıklaması*")]
        public string Explanation { get; set; }
    }
}
