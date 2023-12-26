using humanResourceProject.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using humanResourceProject.Models.Validations;

namespace humanResourceProject.Models.DTOs
{
    public class LeaveDTO
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "İzin türü boş geçilemez!")]
        [DisplayName("İzin Türü*")]
        public LeaveType LeaveType { get; set; }
        [Required(ErrorMessage = "İzin başlangıç tarihi boş geçilemez!")]
        [DisplayName("İzin Başlangıç Tarihi*")]
        public DateTime StartDateOfLeave { get; set; }
        [Required(ErrorMessage = "İzin bitiş tarihi boş geçilemez!")]
        [DisplayName("İzin Bitiş Tarihi*")]
        public DateTime EndDateOfLeave { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ManagerId { get; set; }
        //public Status Status { get; set; } = Status.Active;
        public DateTime CreateDate { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public RequestStatus LeaveStatus { get; set; }
        [Required(ErrorMessage = "İzin gün miktarı boş geçilemez!")]
        [DisplayName("İzinli Gün Miktarı*")]
        public decimal DaysOfLeave { get; set; }
        [Required(ErrorMessage = "İzin açıklaması boş geçilemez!")]
        [DisplayName("İzin Açıklaması*")]
        public string Explanation { get; set; }
        public string ManagerFullName { get; set; }
        public string ManagerEmail { get; set; }
    }
}
