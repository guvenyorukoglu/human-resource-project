using humanResourceProject.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Models.VMs
{
    public class DashbordVM
    {
        public List<Leave>? Leaves { get; set; }
        public List<Advance>? Advances { get; set; }
        public List<Expense>? Expenses { get; set; }

        public Company Company { get; set; }


    }
}
