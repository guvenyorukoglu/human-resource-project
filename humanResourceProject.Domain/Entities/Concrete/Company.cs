using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace humanResourceProject.Domain.Entities.Concrete
{
    public class Company : IBaseEntity
    {
        public Company()
        {
            Employees = new HashSet<AppUser>();
            Departments = new HashSet<Department>();
            Jobs = new HashSet<Job>();
        }
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public int NumberOfEmployees { get; set; }
        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<AppUser> Employees { get; set; }
        public ICollection<Department> Departments { get; set; }
        public ICollection<Job> Jobs { get; set; }
        public ICollection<Possession> Possessions { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Active;
        public RequestStatus CompanyStatus { get; set; }
    }
}