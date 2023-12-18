using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.DTOs
{
    public class LeaveDTO
    {
        public Guid Id { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ManagerId { get; set; }
        public Status Status { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
