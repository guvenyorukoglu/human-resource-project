namespace humanResourceProject.Models.VMs
{
    public class DashboardExpenseVM
    {
        public List<DashboardMyExpensesVM> MyExpenses { get; set; }
        public List<DashboardExpensesToBeCompletedByManagerVM> ExpensesToBeCompletedByManager { get; set; }
    }
}
