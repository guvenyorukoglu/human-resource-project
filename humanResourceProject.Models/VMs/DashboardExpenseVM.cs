namespace humanResourceProject.Models.VMs
{
    public class DashboardExpenseVM
    {
        public List<DashboardExpensesVM> MyExpenses { get; set; }
        public List<DashboardExpensesVM> ExpensesToBeCompletedByManager { get; set; }
    }
}
