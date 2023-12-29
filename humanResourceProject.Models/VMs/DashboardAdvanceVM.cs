using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class DashboardAdvanceVM
    {
        public List<DashboardAdvancesVM> MyAdvances { get; set; }
        public List<DashboardAdvancesVM> AdvancesToBeCompletedByManager { get; set; }
    }
}
