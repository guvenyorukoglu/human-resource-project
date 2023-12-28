using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class DashboardAdvancesVM
    {
        public string AdvanceNo { get; set; }
        public decimal AmountOfAdvance { get; set; }
        public DateTime CreateDate { get; set; }
        public RequestStatus AdvanceStatus { get; set; }
    }
}
