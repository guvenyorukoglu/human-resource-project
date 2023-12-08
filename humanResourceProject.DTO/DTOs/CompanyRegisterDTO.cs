using humanResourceProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace humanResourceProject.DTO.DTOs
{
    public class CompanyRegisterDTO
    {
       
        public string CompanyName { get; set; }
       
        public string Password { get; set; }

        public string ConfirmedPassword { get; set; }

        public string Adress { get; set; }

        public string Email { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public Status status => Status.Active;

        public string TaxNumber { get; set; }

        public int NumberOfEmployees { get; set; }

    }
}
