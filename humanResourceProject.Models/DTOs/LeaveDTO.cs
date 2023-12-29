using humanResourceProject.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using static humanResourceProject.Models.Validations.DateTimeValidations;
using humanResourceProject.Models.Validations;


namespace humanResourceProject.Models.DTOs
{
    public class LeaveDTO
    {
        public Guid EmployeeId { get; set; }
        [Required(ErrorMessage = "İzin türü boş geçilemez!")]
        [DisplayName("İzin Türü*")]
        public LeaveType LeaveType { get; set; }
        [Required(ErrorMessage = "İzin başlangıç tarihi boş geçilemez!")]
        [DisplayName("İzin Başlangıç Tarihi*")]
        [LeaveDateTimeValidations(ErrorMessage = "İzni şu anki tarihten 3 ay sonraya ya da 3 ay önceye talep edemezsin.")]
        public DateTime StartDateOfLeave { get; set; }
        [Required(ErrorMessage = "İzin bitiş tarihi boş geçilemez!")]
        [DisplayName("İzin Bitiş Tarihi*")]
        [LeaveDateTimeValidations(ErrorMessage = "İzni şu anki tarihten 3 ay sonraya ya da 3 ay önceye talep edemezsin.")]
        public DateTime EndDateOfLeave { get; set; }
        public DateTime CreateDate { get; set; }
        [Required(ErrorMessage = "İzin gün miktarı boş geçilemez!")]
        [DisplayName("İzinli Gün Miktarı*")]
        [DaysOfLeaveValidations(ErrorMessage ="0.5'den büyük değer giriniz!")]
        public decimal DaysOfLeave { get; set; }
        [Required(ErrorMessage = "İzin açıklaması boş geçilemez!")]
        [DisplayName("İzin Açıklaması*")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Explanation { get; set; }
        public string ManagerFullName { get; set; }
        public string ManagerEmail { get; set; }
    }
}
