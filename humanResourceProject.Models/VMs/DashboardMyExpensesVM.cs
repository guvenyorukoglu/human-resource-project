using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class DashboardMyExpensesVM
    {
        public string ExpenseNo { get; set; }
        public decimal AmountOfExpense { get; set; }
        public DateTime CreateDate { get; set; }
        public RequestStatus ExpenseStatus { get; set; }
    }
}
