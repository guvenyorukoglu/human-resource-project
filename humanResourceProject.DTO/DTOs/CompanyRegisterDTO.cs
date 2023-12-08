﻿using humanResourceProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace humanResourceProject.DTO.DTOs
{
    public class CompanyRegisterDTO
    {
        [Required(ErrorMessage = "Şirket Adı alanı boş geçilemez!")]
        [DisplayName("Şirket Adı")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Adres alanı boş geçilemez!")]
        [DisplayName("Adres")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vergi numarası alanı boş geçilemez!")]
        [DisplayName("Vergi Numarası")]
        public string TaxNumber { get; set; }
        [Required(ErrorMessage = "Vergi dairesi alanı boş geçilemez!")]
        [DisplayName("Vergi Dairesi")]
        public string TaxOffice { get; set; }
        [Required(ErrorMessage = "Telefonu alanı boş geçilemez!")]
        [DisplayName("Telefon")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Personel sayısı alanı boş geçilemez!")]
        [DisplayName("Personel Sayısı")]
        public int NumberOfEmployees { get; set; }

    }
}
