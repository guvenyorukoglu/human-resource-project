using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.Validations;
using humanResourceProject.Models.VMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateProfileDTO
    {
        public Guid Id { get; set; }


        public BloodGroup BloodGroup { get; set; }


        [Required(ErrorMessage = "Adres alanı boş geçilemez!")]
        [DisplayName("Adres*")]
        [StringLength(200, ErrorMessage = "Adres en fazla 200 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telefonu alanı boş geçilemez!")]
        [DisplayName("Telefon*")]
        [PhoneValidations(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        public string PhoneNumber { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
    }
}
