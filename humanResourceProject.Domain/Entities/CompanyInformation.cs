using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Domain.Entities
{
    public class CompanyInformation
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public int NumberOfEmployees { get; set; }
        public double TaxNumber { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<AppUser> Employees { get; set; }
    }
}
