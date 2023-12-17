using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Domain.Entities.Concrete
{
    public class Company : IBaseEntity
    {
        public Company()
        {
            //Employees = new HashSet<AppUser>();
            Departments = new HashSet<Department>();
        }
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public int NumberOfEmployees { get; set; }
        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        //public ICollection<AppUser> Employees { get; set; }
        public ICollection<Department> Departments { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}