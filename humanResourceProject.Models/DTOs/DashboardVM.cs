using humanResourceProject.Models.VMs;

namespace humanResourceProject.Models.DTOs
{
    public class DashboardVM
    {
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public List<LeavePersonnelVM> DashboardLeaves { get; set; }
        public List<AdvancePersonnelVM> DashboardAdvances { get; set; }
        public List<ExpensePersonnelVM> DashboardExpenses { get; set; }
    }
}
