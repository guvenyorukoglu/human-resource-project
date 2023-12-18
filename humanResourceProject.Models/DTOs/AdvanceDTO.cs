using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.DTOs
{
    public class AdvanceDTO
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public decimal AdvanceAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public RequestStatus AdvanceStatus { get; set; }
        public Status Status { get; set; }
        public AdvanceType AdvanceType { get; set; }
        public string Description { get; set; }
    }
}
