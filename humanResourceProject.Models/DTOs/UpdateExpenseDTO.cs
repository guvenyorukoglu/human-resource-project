using humanResourceProject.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateExpenseDTO
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez!")]
        [DisplayName("Açıklama*")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Explanation { get; set; }
        [Required(ErrorMessage = "Miktar alanı boş geçilemez!")]
        [DisplayName("Harcama Miktarı*")]
        public decimal AmountOfExpense { get; set; }
        public RequestStatus ExpenseStatus { get; set; }

        [Required(ErrorMessage = "Masraf türü alanı boş geçilemez!")]
        [DisplayName("Masraf Türü*")]
        public ExpenseType ExpenseType { get; set; }
        [Required(ErrorMessage = "Para birimi alanı boş geçilemez!")]
        [DisplayName("Para Birimi*")]
        public Currency Currency { get; set; }

        public string? FilePath { get; set; }
        

        [Required(ErrorMessage = "Masraf tarihi alanı boş geçilemez!")]
        [DisplayName("Masraf Tarihi*")]
        public DateTime DateOfExpense { get; set; }
        
        [Required(ErrorMessage = "Dosya alanı boş geçilemez!")]
        [DisplayName("Dosya*")]
        public IFormFile? UploadPath { get; set; }
    }
}
