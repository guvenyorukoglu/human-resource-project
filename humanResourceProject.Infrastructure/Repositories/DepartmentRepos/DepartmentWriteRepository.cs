using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Infrastructure.Repositories.DepartmentRepos
{
    public class DepartmentWriteRepository:BaseWriteRepository<Department>
    {
        public DepartmentWriteRepository(AppDbContext context):base(context) { }
    
    }
}
