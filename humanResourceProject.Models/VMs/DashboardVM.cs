using humanResourceProject.Domain.Entities.Concrete;

namespace humanResourceProject.Models.VMs
{
    public class DashboardVM
    {
        public List<DashboardLeaveVM>? Leaves { get; set; }
        public List<DashboardAdvanceVM>? Advances { get; set; }
        public List<DashboardExpenseVM>? Expenses { get; set; }
        public CompanyVM Company { get; set; }

        public int? TotalLeaveRequests { get; set; }


    }
}
