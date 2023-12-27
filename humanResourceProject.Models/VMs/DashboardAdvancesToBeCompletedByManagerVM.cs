using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class DashboardAdvancesToBeCompletedByManagerVM
    {
        public string AdvanceNo { get; set; }
        public decimal AmountOfAdvance { get; set; }
        public DateTime CreateDate { get; set; }
        public RequestStatus AdvanceStatus { get; set; }
    }
}
