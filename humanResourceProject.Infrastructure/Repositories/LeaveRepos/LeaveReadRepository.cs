using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Infrastructure.Repositories.LeaveRepos
{
    public class LeaveReadRepository:BaseReadRepository<Leave>
    {
        public LeaveReadRepository(AppDbContext context):base(context) { }
        
    }
}
