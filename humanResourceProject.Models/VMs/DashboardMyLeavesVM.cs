using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class DashboardMyLeavesVM
    {
        public string LeaveNo { get; set; }
        public decimal DaysOfLeave { get; set; }
        public DateTime CreateDate { get; set; }
        public RequestStatus LeaveStatus { get; set; }
    }
}
