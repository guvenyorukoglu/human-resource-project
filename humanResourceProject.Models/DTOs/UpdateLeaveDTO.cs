using humanResourceProject.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using humanResourceProject.Models.Validations;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateLeaveDTO
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        [Required(ErrorMessage = "İzin türü alanı boş geçilemez!")]
        [DisplayName("İzin Türü*")]
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
        [DaysOfLeaveValidations(ErrorMessage = "Lütfen geçerli bir değer giriniz!")]
        public decimal DaysOfLeave { get; set; }
        public DateTime CreateDate { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez!")]
        [DisplayName("Açıklama*")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Explanation { get; set; }
        public string LeaveNo { get; set; }
        public string? RejectReason { get; set; }

    }
}
