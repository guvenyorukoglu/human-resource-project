using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class AdvanceVM
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal AmountOfAdvance { get; set; }
        public Currency Currency { get; set; }
        public DateTime ExpiryDate { get; set; }
        public AdvanceType AdvanceType { get; set; }
        public string Explanation { get; set; }
        public RequestStatus AdvanceStatus { get; set;}
        public DateTime CreateDate { get; set; }
        public string AdvanceNo { get; set; }

    }
}
