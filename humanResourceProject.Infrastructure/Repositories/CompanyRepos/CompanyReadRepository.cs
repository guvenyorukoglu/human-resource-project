using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;

namespace humanResourceProject.Infrastructure.Repositories.CompanyRepos
{
    public class CompanyReadRepository : BaseReadRepository<Company>
    {
        public CompanyReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
