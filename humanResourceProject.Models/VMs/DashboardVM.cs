namespace humanResourceProject.Models.VMs
{
    public class DashboardVM
    {
        public List<DashboardMyLeavesVM>? MyLeaves { get; set; }
        public List<DashboardLeavesToBeCompletedByManagerVM>? LeavesToBeCompletedByManager { get; set; }
        public List<DashboardMyAdvancesVM>? MyAdvances { get; set; }
        public List<DashboardAdvancesToBeCompletedByManagerVM>? AdvancesToBeCompletedByManager { get; set; }
        public List<DashboardMyExpensesVM>? MyExpenses { get; set; }
        public List<DashboardExpensesToBeCompletedByManagerVM>? ExpensesToBeCompletedByManager { get; set; }
        public CompanyVM Company { get; set; }
        public int MyPendingLeavesCount { get; set; }
        public int PendingLeavesCountForManager { get; set; }
        public int MyPendingAdvancesCount { get; set; }
        public int PendingAdvancesCountForManager { get; set; }
        public int MyPendingExpensesCount { get; set; }
        public int PendingExpensesCountForManager { get; set; }

    }
}
