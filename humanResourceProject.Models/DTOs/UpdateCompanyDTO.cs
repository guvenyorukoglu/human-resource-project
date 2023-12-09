using humanResourceProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateCompanyDTO
    {
      
        public string CompanyName { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfEmployees { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
