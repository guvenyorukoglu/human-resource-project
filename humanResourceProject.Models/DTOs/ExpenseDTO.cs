using humanResourceProject.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static humanResourceProject.Models.Validations.DateTimeValidations;

namespace humanResourceProject.Models.DTOs
{
    public class ExpenseDTO
    {
        //public Guid Id { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez!")]
        [DisplayName("Açıklama*")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string? Explanation { get; set; }
        [Required(ErrorMessage = "Miktar alanı boş geçilemez!")]
        [DisplayName("Masraf Miktarı*")]
        //[Range(1, 100000, ErrorMessage = "Masraf miktarı 1 ile 100000 arasında olmalıdır.")]
        public decimal AmountOfExpense { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public Status Status { get; set; }
        //public AppUser Employee { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ManagerId { get; set; }
        public DateTime CreateDate { get; set; }
        [Required(ErrorMessage = "Para birimi alanı boş geçilemez!")]
        [DisplayName("Para Birimi*")]
        public Currency Currency { get; set; }
        [Required(ErrorMessage = "Dosya alanı boş geçilemez!")]
        [DisplayName("Dosya*")]
        public IFormFile? UploadPath { get; set; }
        public string? FilePath { get; set; }

        [Required(ErrorMessage = "Masraf türü alanı boş geçilemez!")]
        [DisplayName("Masraf Türü*")]
        public ExpenseType ExpenseType { get; set; }

        public RequestStatus ExpenseStatus { get; set; }

        [Required(ErrorMessage = "Masraf tarihi alanı boş geçilemez!")]
        [DisplayName("Masraf Tarihi*")]
       
        public DateTime DateOfExpense { get; set; }
        public string ManagerFullName { get; set; }
        public string ManagerEmail { get; set; }

    }
}
