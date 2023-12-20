using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class ExpensePersonnelVM
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez!")]
        [DisplayName("Açıklama*")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Miktar alanı boş geçilemez!")]
        [DisplayName("Harcama Miktarı*")]
        public decimal AmountOfExpense { get; set; }
        [DisplayName("Harcama Tarihi*")]
        public DateTime? DateOfExpense { get; set; }
        public string? FilePath { get; set; }
        [DisplayName("Masraf Fotoğrafı")]
        public IFormFile? UploadPath { get; set; }

        public Status Status { get; set; }
        public RequestStatus ExpenseStatus { get; set; }
    }
}
