﻿using humanResourceProject.Domain.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateAdvanceDTO
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        //[Required(ErrorMessage = "İsim alanı boş geçilemez!")]
        //[DisplayName("İsim*")]
        //[RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        //public string EmployeeName { get; set; }
        //[Required(ErrorMessage = "Soyisim alanı boş geçilemez!")]
        //[DisplayName("Soyisim*")]
        //[RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        //public string EmployeeSurname { get; set; }
        [Required(ErrorMessage = "Miktar alanı boş geçilemez!")]
        [DisplayName("Avans Miktarı*")]
        public decimal AmountOfAdvance { get; set; }
        [Required(ErrorMessage = "Avans tarihi boş geçilemez!")]
        [DisplayName("Son İstenen Tarihi*")]
        public DateTime ExpiryDate { get; set; }
        //public DateTime UpdateDate { get; set; }
        public RequestStatus AdvanceStatus { get; set; }
        //public Status Status { get; set; }
        [Required(ErrorMessage = "Avans tipi alanı boş geçilemez!")]
        [DisplayName("Avans Tipi*")]
        public AdvanceType AdvanceType { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez!")]
        [DisplayName("Açıklama*")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Explanation { get; set; }
        [Required(ErrorMessage = "Para birimi alanı boş geçilemez!")]
        [DisplayName("Para Birimi*")]
        public Currency Currency { get; set; }
    }
}
