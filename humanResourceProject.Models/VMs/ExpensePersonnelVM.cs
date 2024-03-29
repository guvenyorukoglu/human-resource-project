﻿using humanResourceProject.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.VMs
{
    public class ExpensePersonnelVM
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez!")]
        [DisplayName("Açıklama*")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Explanation { get; set; }
        [Required(ErrorMessage = "Miktar alanı boş geçilemez!")]
        [DisplayName("Masraf Miktarı*")]
        public decimal AmountOfExpense { get; set; }
        [DisplayName("Masraf Tarihi*")]
        public DateTime DateOfExpense { get; set; }
        public string FilePath { get; set; }
        [DisplayName("Masraf Fotoğrafı")]
        public IFormFile? UploadPath { get; set; }
        public RequestStatus ExpenseStatus { get; set; }
        public Guid EmployeeId { get; set; }
        public Currency Currency { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public DateTime CreateDate { get; set; }
        public string ManagerFullName { get; set; }
        public string ExpenseNo { get; set; }
        public Status Status { get; set; }
    }
}
