namespace humanResourceProject.Models.VMs
{
    public class DashboardLeaveVM
    {
        public List<DashboardMyLeavesVM> MyLeaves { get; set; }
        public List<DashboardLeavesToBeCompletedByManagerVM> LeavesToBeCompletedByManager { get; set; }
    }
}
