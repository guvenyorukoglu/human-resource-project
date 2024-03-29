using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class LeaveVM
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDateOfLeave { get; set; }
        public DateTime EndDateOfLeave { get; set; }
        public Guid ManagerId { get; set; }

        public string? Explanation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal DaysOfLeave { get; set; }
        public RequestStatus LeaveStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public string LeaveNo { get; set; }
    }
}
