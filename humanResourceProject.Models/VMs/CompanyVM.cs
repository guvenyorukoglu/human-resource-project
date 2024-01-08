using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class CompanyVM
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public int NumberOfEmployees { get; set; }
        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public Status Status { get; set; }
        public string? RejectReason { get; set; }
        public RequestStatus CompanyStatus { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<AppUser> Employees { get; set; }

    }
}
