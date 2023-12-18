using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Domain.IRepository.DepartmentRepo
{
    public interface IDepartmentReadRepository:IBaseReadRepository<Department>
    {
    }
}
