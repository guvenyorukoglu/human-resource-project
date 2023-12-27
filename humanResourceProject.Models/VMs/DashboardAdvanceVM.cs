using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class DashboardAdvanceVM
    {
        public List<DashboardMyAdvancesVM> MyAdvances { get; set; }
        public List<DashboardAdvancesToBeCompletedByManagerVM> AdvancesToBeCompletedByManager { get; set; }
    }
}
