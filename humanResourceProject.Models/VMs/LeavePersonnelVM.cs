using humanResourceProject.Domain.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.VMs
{
    public class LeavePersonnelVM
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        [Required(ErrorMessage = "Başlangıç Tarihi boş geçilemez.")]
        [DisplayName("Başlangıç Tarihi*")]
        public DateTime StartDateOfLeave { get; set; }

        [Required(ErrorMessage = "Bitiş Tarihi boş geçilemez.")]
        [DisplayName("Bitiş Tarihi*")]
        public DateTime EndDateOfLeave { get; set; }

        [Required(ErrorMessage = "İzin Türü boş geçilemez.")]
        [DisplayName("İzin Türü*")]
        public LeaveType LeaveType { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez!")]
        [DisplayName("Açıklama")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Explanation { get; set; }

        [Required(ErrorMessage = "İzin Gün Sayısı boş geçilemez.")]
        [DisplayName("İzin Gün Sayısı*")]
        public decimal DaysOfLeave { get; set; }
        public RequestStatus LeaveStatus { get; set; }
        //public Status Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string ManagerFullName { get; set; }
        public string LeaveNo { get; set; }
        public Status Status { get; set; }

    }
}
