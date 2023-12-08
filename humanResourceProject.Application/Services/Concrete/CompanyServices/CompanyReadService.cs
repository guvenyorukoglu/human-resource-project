using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Application.Services.Concrete.CompanyServices
{
    public class CompanyReadService : BaseReadService<Company>, ICompanyReadService
    {
        public CompanyReadService(IBaseReadRepository<Company> readRepository) : base(readRepository)
        {
        }
    }
}
