using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class LeaveVM
    {
        public Guid Id { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ManagerId { get; set; }
        public Guid DepartmentId { get; set; }
        public string? Explanation { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public decimal DaysOfLeave { get; set; }
        public RequestStatus LeaveStatus { get; set; }
    }
}
