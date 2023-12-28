namespace humanResourceProject.Models.VMs
{
    public class DashboardVM
    {
        public List<DashboardLeavesVM>? MyLeaves { get; set; }
        public List<DashboardLeavesVM>? LeavesToBeCompletedByManager { get; set; }
        public List<DashboardAdvancesVM>? MyAdvances { get; set; }
        public List<DashboardAdvancesVM>? AdvancesToBeCompletedByManager { get; set; }
        public List<DashboardExpensesVM>? MyExpenses { get; set; }
        public List<DashboardExpensesVM>? ExpensesToBeCompletedByManager { get; set; }
        public CompanyVM Company { get; set; }
        public int MyPendingLeavesCount { get; set; }
        public int PendingLeavesCountForManager { get; set; }
        public int MyPendingAdvancesCount { get; set; }
        public int PendingAdvancesCountForManager { get; set; }
        public int MyPendingExpensesCount { get; set; }
        public int PendingExpensesCountForManager { get; set; }

        public int? TotalLeaveRequests { get; set; }


    }
}
