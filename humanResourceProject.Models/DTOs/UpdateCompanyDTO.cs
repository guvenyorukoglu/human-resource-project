using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace humanResourceProject.Models.DTOs
{
    public class UpdateCompanyDTO
    {

        public Guid Id { get; set; }

        [Required(ErrorMessage = "Şirket Adı alanı boş geçilemez!")]
        [DisplayName("Şirket Adı")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Adres alanı boş geçilemez!")]
        [DisplayName("Adres")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Telefonu alanı boş geçilemez!")]
        [DisplayName("Telefon")]
        [PhoneValidations(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Personel sayısı alanı boş geçilemez!")]
        [DisplayName("Personel Sayısı")]
        public int NumberOfEmployees { get; set; }
        public DateTime UpdateDate { get; set; }
        public Status Status { get; set; }

    }
}
