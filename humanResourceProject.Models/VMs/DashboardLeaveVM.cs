namespace humanResourceProject.Models.VMs
{
    public class DashboardLeaveVM
    {
        public List<DashboardLeavesVM> MyLeaves { get; set; }
        public List<DashboardLeavesVM> LeavesToBeCompletedByManager { get; set; }
    }
}
