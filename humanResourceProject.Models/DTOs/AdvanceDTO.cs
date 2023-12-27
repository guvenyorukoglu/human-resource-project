using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static humanResourceProject.Models.Validations.DateTimeValidations;


namespace humanResourceProject.Models.DTOs
{
    public class AdvanceDTO
    {
        public Guid EmployeeId { get; set; }
        public string ManagerFullName { get; set; }
        public string ManagerEmail { get; set; }
        [Required(ErrorMessage = "Miktar alanı boş geçilemez!")]
        [DisplayName("Avans Miktarı*")]
        public decimal AmountOfAdvance { get; set; }
        [Required(ErrorMessage = "Avans tarihi boş geçilemez!")]
        [DisplayName("Son İstenen Tarihi*")]
        [AdvanceMinDateTimeValidations(ErrorMessage = "Avansı şu anki tarihten önce talep edemezsin.")]
        public DateTime ExpiryDate { get; set; }
        [Required(ErrorMessage = "Avans tipi alanı boş geçilemez!")]
        [DisplayName("Avans Tipi*")]
        public AdvanceType AdvanceType { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez!")]
        [DisplayName("Açıklama*")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Explanation { get; set; }
        public DateTime CreateDate { get; set; }
        [Required(ErrorMessage = "Para birimi alanı boş geçilemez!")]
        [DisplayName("Para Birimi*")]
        public Currency Currency { get; set; }
    }
}
