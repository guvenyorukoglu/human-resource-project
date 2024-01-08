using humanResourceProject.Domain.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateAdvanceDTO
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez!")]
        [DisplayName("Açıklama*")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Explanation { get; set; }

        [Required(ErrorMessage = "Miktar alanı boş geçilemez!")]
        [DisplayName("Avans Miktarı*")]
        [Range(100, 100000, ErrorMessage = "Avans miktarı 100 ile 100000 arasında olmalıdır.")]
        public decimal AmountOfAdvance { get; set; }
        [Required(ErrorMessage = "Avans tarihi boş geçilemez!")]
        [DisplayName("Son İstenen Tarihi*")]
        public DateTime ExpiryDate { get; set; }
        public RequestStatus AdvanceStatus { get; set; }

        [Required(ErrorMessage = "Avans türü alanı boş geçilemez!")]
        [DisplayName("Avans Türü*")]
        public AdvanceType AdvanceType { get; set; }
        [Required(ErrorMessage = "Para birimi alanı boş geçilemez!")]
        [DisplayName("Para Birimi*")]
        public Currency Currency { get; set; }
        public DateTime CreateDate { get; set; }
        public string AdvanceNo { get; set; }
        public string? RejectReason { get; set; }
    }
}
